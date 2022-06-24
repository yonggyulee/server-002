using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListSegmentationGts;

public class ListSegmentationGtsHandler : GtDatasetHandler, IRequestHandler<ListSegmentationGtsCommand, ListSegmentationGtsResponse>
{
    public ListSegmentationGtsHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListSegmentationGtsHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }

    public async Task<ListSegmentationGtsResponse> Handle(ListSegmentationGtsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.SegmentationGts
            .AsNoTracking()
            .Include(s => s.GtDataset.Volume)
            .AsPagedResultAsync(request.QueryParameter, cancellationToken);

        var gtDataset = items.Select(gt => gt.GtDataset).Distinct().First();
        await using var @lock =
            await _AcquireLockByGtDatasetAsync(gtDataset, request.LockTimeoutSec, false, cancellationToken);

        var gts = (await Task.WhenAll(items.Select(async gt =>
        {
            gt.Buffer =
                await _fileStorage.GetFileBufferAsync(
                    Path.Combine(gt.GtDataset.Volume.Uri, gt.GtDataset.DirectoryName),
                    gt.Filename, cancellationToken);
            return gt;
        }))).Select(s => _mapper.From(s).AdaptToType<SegmentationGt>());

        return _mapper.From((request, gts, count)).AdaptToType<ListSegmentationGtsResponse>();
    }
}