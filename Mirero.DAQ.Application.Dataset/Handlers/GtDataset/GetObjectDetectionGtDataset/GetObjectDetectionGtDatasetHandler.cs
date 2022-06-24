using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetObjectDetectionGtDataset;

public class GetObjectDetectionGtDatasetHandler : GtDatasetHandler, IRequestHandler<GetObjectDetectionGtDatasetCommand, ObjectDetectionGtDataset>
{
    public GetObjectDetectionGtDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetObjectDetectionGtDatasetHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }

    public async Task<ObjectDetectionGtDataset> Handle(GetObjectDetectionGtDatasetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var dataset = await _dbContext.ObjectDetectionGtDatasets
                          .AsNoTracking()
                          .SingleOrDefaultAsync(
                              d => d.Id == request.ObjectDetectionGtDatasetId,
                              cancellationToken)
                      ?? throw new NotImplementedException();

        return _mapper.From(dataset).AdaptToType<ObjectDetectionGtDataset>();
    }
}