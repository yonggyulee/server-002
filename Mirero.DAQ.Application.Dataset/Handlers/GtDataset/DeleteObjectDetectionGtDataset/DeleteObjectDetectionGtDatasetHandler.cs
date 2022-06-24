using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteObjectDetectionGtDataset;

public class DeleteObjectDetectionGtDatasetHandler : GtDatasetHandler, IRequestHandler<DeleteObjectDetectionGtDatasetCommand, ObjectDetectionGtDataset>
{
    public DeleteObjectDetectionGtDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteObjectDetectionGtDatasetHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }

    public async Task<ObjectDetectionGtDataset> Handle(DeleteObjectDetectionGtDatasetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var selectedDataset = await _dbContext.ObjectDetectionGtDatasets
                                  .Include(d => d.Volume)
                                  .SingleOrDefaultAsync(
                                      d => d.Id == request.ObjectDetectionGtDatasetId,
                                      cancellationToken)
                              ?? throw new NotImplementedException();

        await using var @lock =
            await _AcquireLockByGtDatasetAsync(selectedDataset, request.LockTimeoutSec, true, cancellationToken);

        _dbContext.ObjectDetectionGtDatasets.Remove(selectedDataset);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _fileStorage.DeleteFolder(Path.Combine(selectedDataset.Volume.Uri, selectedDataset.DirectoryName));

        return _mapper.From(selectedDataset).AdaptToType<ObjectDetectionGtDataset>();
    }
}