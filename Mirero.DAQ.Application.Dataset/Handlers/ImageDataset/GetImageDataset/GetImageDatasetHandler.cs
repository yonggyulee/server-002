using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using ImageDatasetDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ImageDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.GetImageDataset;

public class GetImageDatasetHandler : DatasetHandlerBase, IRequestHandler<GetImageDatasetCommand, Domain.Dataset.Protos.V1.ImageDataset>
{
    public GetImageDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetImageDatasetHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ImageDatasetDto> Handle(GetImageDatasetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        await using var @lock = await _lockProvider.AcquireReadLockAsync(GenerateLockId<ImageDatasetDto>(request.DatasetId),
            cancellationToken: cancellationToken);
        var dataset = await _dbContext.ImageDatasets
                          .Include(d => d.Volume)
                          .SingleOrDefaultAsync(
                              d => d.Id == request.DatasetId, cancellationToken: cancellationToken)
                      ?? throw new NotImplementedException();

        return _mapper.From(dataset).AdaptToType<ImageDatasetDto>();
    }
}