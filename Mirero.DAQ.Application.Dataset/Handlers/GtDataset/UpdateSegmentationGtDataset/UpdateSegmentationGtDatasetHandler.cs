using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateSegmentationGtDataset;

public class UpdateSegmentationGtDatasetHandler : GtDatasetHandler, IRequestHandler<UpdateSegmentationGtDatasetCommand, SegmentationGtDataset>
{
    private readonly RequesterContext _requesterContext;
    public UpdateSegmentationGtDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<UpdateSegmentationGtDatasetHandler> logger,
        IFileStorage fileStorage, IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
        _requesterContext = requesterContext;
    }
    public async Task<SegmentationGtDataset> Handle(UpdateSegmentationGtDatasetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        var selectedDataset = await _dbContext.SegmentationGtDatasets
                                  .Include(v => v.Volume)
                                  .SingleOrDefaultAsync(d => d.Id == request.Id,
                                      cancellationToken)
                              ?? throw new NotImplementedException();

        await using var @lock =
            await _AcquireLockByGtDatasetAsync(selectedDataset, request.LockTimeoutSec, true, cancellationToken);

        var isChangedVolume = request.VolumeId != selectedDataset.VolumeId;

        var isChangedUri = isChangedVolume || request.DirectoryName != selectedDataset.DirectoryName;

        var (originUri, newUri) = ("", "");

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

        return _mapper.From(selectedDataset).AdaptToType<SegmentationGtDataset>();
    }
}
