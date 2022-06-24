using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using ImageDatasetDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ImageDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.GetSample;

public class GetSampleHandler : SampleHandler, IRequestHandler<GetSampleCommand, Sample>
{
    public GetSampleHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetSampleHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<Sample> Handle(GetSampleCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var sample = await _dbContext.Samples
                         .Include(s => s.ImageDataset.Volume)
                         .Include(s => s.Images)
                         .AsNoTracking()
                         .SingleOrDefaultAsync(s =>
                                 s.Id == request.SampleId && s.DatasetId == request.DatasetId,
                             cancellationToken: cancellationToken)
                     ?? throw new NotImplementedException();

        await using var @lock = await _lockProvider.AcquireReadLockAsync(GenerateLockId<ImageDatasetDto>(sample.DatasetId),
            request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        return await _ToSampleWithImageAsync(sample, cancellationToken);
    }
}