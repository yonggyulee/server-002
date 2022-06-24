using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteSegmentationGtDataset;

public class DeleteSegmentationGtDatasetHandler : GtDatasetHandler, IRequestHandler<DeleteSegmentationGtDatasetCommand, SegmentationGtDataset>
{
    public DeleteSegmentationGtDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteSegmentationGtDatasetHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }

    public async Task<SegmentationGtDataset> Handle(DeleteSegmentationGtDatasetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var selectedDataset = await _dbContext.SegmentationGtDatasets
                                  .Include(d => d.Volume)
                                  .SingleOrDefaultAsync(
                                      d => d.Id == request.SegmentationGtDatasetId,
                                      cancellationToken)
                              ?? throw new NotImplementedException();

        await using var @lock =
            await _AcquireLockByGtDatasetAsync(selectedDataset, request.LockTimeoutSec, true, cancellationToken);

        _dbContext.SegmentationGtDatasets.Remove(selectedDataset);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _fileStorage.DeleteFolder(Path.Combine(selectedDataset.Volume.Uri, selectedDataset.DirectoryName));

        return _mapper.From(selectedDataset).AdaptToType<SegmentationGtDataset>();
    }
}