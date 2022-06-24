using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetSegmentationGt;

public class GetSegmentationGtHandler : GtDatasetHandler, IRequestHandler<GetSegmentationGtCommand, SegmentationGt>
{
    public GetSegmentationGtHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetSegmentationGtHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }
    public async Task<SegmentationGt> Handle(GetSegmentationGtCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var segmentationGt = await _dbContext.SegmentationGts
                                 .Include(s => s.GtDataset.Volume)
                                 .SingleOrDefaultAsync(
                                     s => s.Id == request.SegmentationGtId,
                                     cancellationToken)
                             ?? throw new NotImplementedException();

        await using var @lock = await _AcquireLockByGtDatasetAsync(segmentationGt.GtDataset, request.LockTimeoutSec,
            false, cancellationToken);

        segmentationGt.Buffer = await _fileStorage.GetFileBufferAsync(
            Path.Combine(segmentationGt.GtDataset.Volume.Uri, segmentationGt.GtDataset.DirectoryName),
            segmentationGt.Filename, cancellationToken);

        return _mapper.From(segmentationGt).AdaptToType<SegmentationGt>();
    }
}