using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Gds;
using Mirero.DAQ.Infrastructure.Storage;
using GdsStatus = Mirero.DAQ.Domain.Gds.Constants.GdsStatus;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.UploadGdsStream;

public class UploadGdsStreamHandler : IRequestHandler<UploadGdsStreamCommand, Empty>
{
    private readonly ILogger _logger;
    private readonly GdsDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public UploadGdsStreamHandler(ILogger<UploadGdsStreamHandler> logger,
        IFileStorage fileStorage,
        IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<Empty> Handle(UploadGdsStreamCommand command, CancellationToken cancellationToken)
    {
        long totalReadBytes = 0;
        long fileSizeBytes = 0;
        var lastProgress = GdsStatus.NoFile;
        FileStream? fs = null;
        Domain.Gds.Entities.Gds? gds = null;
        string? deleteUri = null;
        try
        {
            while (await command.RequestStream.MoveNext())
            {
                var stream = command.RequestStream.Current;
                totalReadBytes += stream.DataInfo.ChunkSize;
                if (stream.DataInfo.ChunkNum == 0)
                {
                    fileSizeBytes = stream.DataInfo.FileSize;
                    gds = await _dbContext.Gds
                            .SingleOrDefaultAsync(
                                g => g.Id == stream.GdsId,
                                cancellationToken: cancellationToken
                            )
                        ;
                    if (gds == null)
                    {
                        throw new InvalidOperationException($"존재하지 않는 GDS Id 값(={stream.GdsId}) 입니다.");
                    }

                    if (gds.Status is GdsStatus.NoFile or GdsStatus.Fail)
                    {
                        var volume = await _dbContext.Volumes
                                .FirstAsync(
                                    v => v.Id == gds.VolumeId,
                                    cancellationToken: cancellationToken
                                )
                            ;

                        var uri = Path.Combine(volume.Uri, gds.Id.ToString(), gds.Filename);
                        deleteUri = uri;
                        fs = _fileStorage.CreateFileStream(uri);
                    }
                    else
                    {
                        throw new InvalidOperationException("Upload 하려는 Gds 파일이 이미 존재합니다.");
                    }
                }

                if (fs is not null)
                {
                    await fs
                            .WriteAsync(
                                stream.Buffer.ToByteArray(),
                                cancellationToken
                            )
                        ;
                }

                var progress = GdsStatus.Process(totalReadBytes, fileSizeBytes);
                if (lastProgress != progress)
                {
                    var isDone = totalReadBytes == fileSizeBytes;
                    if (gds != null)
                    {
                        gds.Status = isDone ? GdsStatus.Success : progress;
                    }

                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

                lastProgress = progress;
                _logger.LogDebug("FileName : {FileName} / chunkNum : {ChunkNum} / Progress: {Progress}",
                    stream.DataInfo.Filename, stream.DataInfo.ChunkNum, progress);
            }

            if (fileSizeBytes != totalReadBytes)
            {
                throw new InvalidOperationException(
                    $"Upload 작업 중 파일 크기 {fileSizeBytes} bytes 전체를 수신하지 못했습니다(수신된 파일 크기 = {totalReadBytes}");
            }
        }
        finally
        {
            if (fs != null)
            {
                await fs.DisposeAsync();
            }

            try
            {
                var requestStreamDone = fileSizeBytes == totalReadBytes;
                if (!requestStreamDone)
                {
                    if (gds != null) gds.Status = GdsStatus.Fail;
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    if (!string.IsNullOrEmpty(deleteUri) && _fileStorage.FileExists(deleteUri))
                    {
                        await _fileStorage.DeleteFile(deleteUri, cancellationToken);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Upload 작업 완료 처리 중 오류 발생");
            }
        }

        return new Empty();
    }
}