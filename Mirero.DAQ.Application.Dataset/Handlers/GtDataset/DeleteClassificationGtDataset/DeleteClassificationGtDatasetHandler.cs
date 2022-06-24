using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteClassificationGtDataset;

public class DeleteClassificationGtDatasetHandler : GtDatasetHandler, IRequestHandler<DeleteClassificationGtDatasetCommand, ClassificationGtDataset>
{
    public DeleteClassificationGtDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteClassificationGtDatasetHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }

    public async Task<ClassificationGtDataset> Handle(DeleteClassificationGtDatasetCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        var selectedDataset = await _dbContext.ClassificationGtDatasets
                                  .Include(d => d.Volume)
                                  .SingleOrDefaultAsync(
                                      d => d.Id == request.ClassificationGtDatasetId,
                                      cancellationToken)
                              ?? throw new NotImplementedException();

        //await using var @lock = await AcquireLockByGtDatasetAsync(selectedDataset, true, cancellationToken);

        _dbContext.ClassificationGtDatasets.Remove(selectedDataset);
        await _dbContext.SaveChangesAsync(cancellationToken);

        //_fileStorage.DeleteFolder(Path.Combine(selectedDataset.Volume.Uri, selectedDataset.DirectoryName));

        return _mapper.From(selectedDataset).AdaptToType<ClassificationGtDataset>();
    }
}