using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using GtDatasetEntity = Mirero.DAQ.Domain.Dataset.Entities.GtDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset;

public class GtDatasetHandler : DatasetHandlerBase
{
    public GtDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory, IPostgresLockProviderFactory lockProviderFactory, ILogger<DatasetHandlerBase> logger, IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    protected async Task<ILock> _AcquireLockByGtDatasetAsync(GtDatasetEntity dataset, double timeoutSeconds = default, bool isWrite = false, CancellationToken cancellationToken = default)
    {
        var lockIds = new List<string>
        {
            GenerateLockId<Domain.Dataset.Protos.V1.ImageDataset>(dataset.ImageDatasetId),
            GenerateLockId<ClassCodeSet>(dataset.ClassCodeSetId)
        };

        return isWrite
            ? await _lockProvider.AcquireWriteLockAsync(lockIds, timeoutSeconds, cancellationToken: cancellationToken)
            : await _lockProvider.AcquireReadLockAsync(lockIds, timeoutSeconds, cancellationToken: cancellationToken);
    }

    protected async Task<ILock> _AcquireLockByGtDatasetAsync(IEnumerable<GtDatasetEntity> datasets,
        double timeoutSeconds = default, bool isWrite = false, CancellationToken cancellationToken = default)
    {
        var lockIds = datasets.Select(d => new List<string>
        {
            GenerateLockId<Domain.Dataset.Protos.V1.ImageDataset>(d.ImageDatasetId),
            GenerateLockId<ClassCodeSet>(d.ClassCodeSetId)
        }).SelectMany(d => d);

        return isWrite
            ? await _lockProvider.AcquireWriteLockAsync(lockIds, timeoutSeconds, cancellationToken: cancellationToken)
            : await _lockProvider.AcquireReadLockAsync(lockIds, timeoutSeconds, cancellationToken: cancellationToken);
    }
}