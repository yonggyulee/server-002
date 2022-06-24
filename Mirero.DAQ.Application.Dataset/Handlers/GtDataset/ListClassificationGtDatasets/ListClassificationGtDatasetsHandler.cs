using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListClassificationGtDatasets;

public class ListClassificationGtDatasetsHandler : GtDatasetHandler, IRequestHandler<ListClassificationGtDatasetsCommand, ListClassificationGtDatasetsResponse>
{
    public ListClassificationGtDatasetsHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListClassificationGtDatasetsHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }

    public async Task<ListClassificationGtDatasetsResponse> Handle(ListClassificationGtDatasetsCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.ClassificationGtDatasets
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter,
                d => _mapper.From(d).AdaptToType<ClassificationGtDataset>(),
                cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListClassificationGtDatasetsResponse>();
    }
}