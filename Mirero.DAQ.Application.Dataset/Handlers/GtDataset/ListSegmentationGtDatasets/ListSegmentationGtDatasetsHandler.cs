using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListSegmentationGtDatasets;

public class ListSegmentationGtDatasetsHandler : GtDatasetHandler, IRequestHandler<ListSegmentationGtDatasetsCommand, ListSegmentationGtDatasetsResponse>
{
    public ListSegmentationGtDatasetsHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListSegmentationGtDatasetsHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }
    public async Task<ListSegmentationGtDatasetsResponse> Handle(ListSegmentationGtDatasetsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.SegmentationGtDatasets
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter,
                d => _mapper.From(d).AdaptToType<SegmentationGtDataset>(),
                cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListSegmentationGtDatasetsResponse>();
    }
}