using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListGtDatasets;

public class ListGtDatasetsHandler : GtDatasetHandler, IRequestHandler<ListGtDatasetsCommand, ListGtDatasetsResponse>
{
    public ListGtDatasetsHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListGtDatasetsHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }

    public async Task<ListGtDatasetsResponse> Handle(ListGtDatasetsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.GtDatasets
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter, d => _mapper.From(d).AdaptToType<Domain.Dataset.Protos.V1.GtDataset>(),
                cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListGtDatasetsResponse>();
    }
}