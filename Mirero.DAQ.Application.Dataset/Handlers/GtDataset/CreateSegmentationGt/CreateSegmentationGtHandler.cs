using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using SegmentationGtEntity = Mirero.DAQ.Domain.Dataset.Entities.SegmentationGt;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateSegmentationGt;

public class CreateSegmentationGtHandler : GtDatasetHandler, IRequestHandler<CreateSegmentationGtCommand, SegmentationGt>
{
    private readonly RequesterContext _requesterContext;
    public CreateSegmentationGtHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<CreateSegmentationGtHandler> logger,
        IFileStorage fileStorage, IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<SegmentationGt> Handle(CreateSegmentationGtCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var segmentationGt = _mapper.From(request).AdaptToType<SegmentationGtEntity>();
        
        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        var gtDataset =
            await _dbContext.GtDatasets.Include(d => d.Volume)
                .SingleOrDefaultAsync(d => d.Id == segmentationGt.DatasetId, cancellationToken) ??
            throw new NullReferenceException();

        await using var @lock =
            await _AcquireLockByGtDatasetAsync(gtDataset, request.LockTimeoutSec, true, cancellationToken);

        if (segmentationGt.Buffer == null)
        {
            throw new ArgumentNullException();
        }

        var datasetUri = Path.Combine(gtDataset.Volume.Uri, gtDataset.DirectoryName);

        await _fileStorage.SaveFileAsync(datasetUri,
            segmentationGt.Filename, segmentationGt.Buffer, cancellationToken);

        try
        {
            await _dbContext.SegmentationGts.AddAsync(segmentationGt, cancellationToken);
            gtDataset.CreateUser = _requesterContext.UserName;
            gtDataset.UpdateUser = _requesterContext.UserName;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
    {
            await _fileStorage.DeleteFileAsync(datasetUri, segmentationGt.Filename, cancellationToken);
            throw;
        }

        return _mapper.From(segmentationGt).AdaptToType<SegmentationGt>();
    }
}
