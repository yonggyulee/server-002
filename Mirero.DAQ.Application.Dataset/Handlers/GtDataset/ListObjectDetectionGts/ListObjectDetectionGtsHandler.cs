using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListObjectDetectionGts;

public class ListObjectDetectionGtsHandler : GtDatasetHandler, IRequestHandler<ListObjectDetectionGtsCommand, ListObjectDetectionGtsResponse>
{
    public ListObjectDetectionGtsHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListObjectDetectionGtsHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }

    public async Task<ListObjectDetectionGtsResponse> Handle(ListObjectDetectionGtsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.ObjectDetectionGts
            .AsNoTracking()
            .Include(od => od.GtDataset.Volume)
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
        }))).Select(od => _mapper.From(od).AdaptToType<ObjectDetectionGt>());

        return _mapper.From((request, gts, count)).AdaptToType<ListObjectDetectionGtsResponse>();
    }
}