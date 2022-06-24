using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetClassificationGtDataset;

public class GetClassificationGtDatasetHandler : GtDatasetHandler, IRequestHandler<GetClassificationGtDatasetCommand, ClassificationGtDataset>
{
    public GetClassificationGtDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetClassificationGtDatasetHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }
    public async Task<ClassificationGtDataset> Handle(GetClassificationGtDatasetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var dataset = await _dbContext.ClassificationGtDatasets
                          .AsNoTracking()
                          .SingleOrDefaultAsync(
                              d => d.Id == request.ClassificationGtDatasetId,
                              cancellationToken)
                      ?? throw new NotImplementedException();

        return _mapper.From(dataset).AdaptToType<ClassificationGtDataset>();
    }
}