using System.Linq.Dynamic.Core;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using GtDatasetEntity = Mirero.DAQ.Domain.Dataset.Entities.GtDataset;
using ImageEntity = Mirero.DAQ.Domain.Dataset.Entities.Image;

namespace Mirero.DAQ.Application.Dataset.Handlers.TrainingData;

public class TrainingDataHandler : GtDatasetHandler
{
    public TrainingDataHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<TrainingDataHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    protected async Task<ILock> _AcquireMultipleLockByTrainingData<TGtDatasetEntity>(string where,
        double timeoutSeconds = default, CancellationToken cancellationToken = default)
        where TGtDatasetEntity : GtDatasetEntity
    {
        var datasetIds = await _dbContext.Images.Where(where).Select(s => s.DatasetId).Distinct()
            .ToListAsync(cancellationToken);

        if (datasetIds.Count == 0) throw new NotImplementedException();

        var lockGtDatasets = await _dbContext.Set<TGtDatasetEntity>()
            .Where("d => @0.Contains(d.ImageDatasetId)", datasetIds)
            //.Where(d => datasetIds.Contains(d.ImageDatasetId))
            .ToListAsync(cancellationToken);

        return await _AcquireLockByGtDatasetAsync(lockGtDatasets, timeoutSeconds, false, cancellationToken);
    }

    protected async Task<Image> _ToImageAsync(ImageEntity image, CancellationToken cancellationToken)
    {
        var uri = Path.Combine(image.Sample.ImageDataset.Volume.Uri, image.Sample.ImageDataset.DirectoryName);
        image.Buffer = await _fileStorage.GetFileBufferAsync(uri, image.Filename, cancellationToken);
        return _mapper.From(image).AdaptToType<Image>();
    }
}