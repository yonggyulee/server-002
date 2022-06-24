using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using ImageDatasetDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ImageDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.UpdateImageDataset;

public class UpdateImageDatasetHandler : DatasetHandlerBase, IRequestHandler<UpdateImageDatasetCommand, ImageDatasetDto>
{
    private readonly RequesterContext _requesterContext;

    public UpdateImageDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<UpdateImageDatasetHandler> logger, IFileStorage fileStorage,
        IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<ImageDatasetDto> Handle(UpdateImageDatasetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        var selectedDataset = await _dbContext.ImageDatasets
                                  .Include(d => d.Volume)
                                  .SingleOrDefaultAsync(
                                      d => d.Id == request.Id,
                                      cancellationToken)
                              ?? throw new NotImplementedException();

        var isChangedVolume = request.VolumeId != selectedDataset.VolumeId;

        var isChangedUri = isChangedVolume || request.DirectoryName != selectedDataset.DirectoryName;

        var (originUri, newUri) = ("", "");

        await using var @lock =
            await _lockProvider.AcquireWriteLockAsync(GenerateLockId<ImageDatasetDto>(selectedDataset.Id),
                request.LockTimeoutSec,
                cancellationToken: cancellationToken);
        if (isChangedUri)
        {
            originUri = Path.Combine(selectedDataset.Volume.Uri, selectedDataset.DirectoryName);

            newUri = isChangedVolume
                ? Path.Combine((await _dbContext.Volumes.FindAsync(
                                    new object?[] { request.VolumeId },
                                    cancellationToken: cancellationToken)
                                ?? throw new NotImplementedException()).Uri,
                    request.DirectoryName)
                : Path.Combine(selectedDataset.Volume.Uri, request.DirectoryName);
            _fileStorage.MoveFolder(originUri, newUri);
        }

        try
        {
            _mapper.From(request).AdaptTo(selectedDataset);
            selectedDataset.CreateUser = _requesterContext.UserName;
            selectedDataset.UpdateUser = _requesterContext.UserName;
            //selectedDataset.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            if (isChangedUri)
            {
                _fileStorage.MoveFolder(newUri, originUri);
            }

            throw;
        }

        return _mapper.From(selectedDataset).AdaptToType<ImageDatasetDto>();
    }
}
