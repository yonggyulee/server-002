using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetSegmentationGtDataset;

public class GetSegmentationGtDatasetHandler : GtDatasetHandler, IRequestHandler<GetSegmentationGtDatasetCommand, SegmentationGtDataset>
{
    public GetSegmentationGtDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetSegmentationGtDatasetHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }
    public async Task<SegmentationGtDataset> Handle(GetSegmentationGtDatasetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var dataset = await _dbContext.SegmentationGtDatasets
                          .AsNoTracking()
                          .SingleOrDefaultAsync(
                              d => d.Id == request.SegmentationGtDatasetId,
                              cancellationToken)
                      ?? throw new NotImplementedException();

        return _mapper.From(dataset).AdaptToType<SegmentationGtDataset>();
    }
}