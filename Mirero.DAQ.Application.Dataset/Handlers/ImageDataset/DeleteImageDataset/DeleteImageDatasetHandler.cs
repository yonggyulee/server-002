using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ImageDatasetDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ImageDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.DeleteImageDataset;

public class DeleteImageDatasetHandler : DatasetHandlerBase, IRequestHandler<DeleteImageDatasetCommand, ImageDatasetDto>
{
    public DeleteImageDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteImageDatasetHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ImageDatasetDto> Handle(DeleteImageDatasetCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        var selectedDataset = await _dbContext.ImageDatasets.Include(d => d.Volume)
                                  .SingleOrDefaultAsync(
                                      d => d.Id == request.DatasetId,
                                      cancellationToken)
                              ?? throw new NotImplementedException();

        await using var @lock =
            await _lockProvider.AcquireWriteLockAsync(GenerateLockId<ImageDatasetDto>(selectedDataset.Id),
                request.LockTimeoutSec,
                cancellationToken: cancellationToken);

        var gtDatasetUris = (await _dbContext.GtDatasets.Include(d => d.Volume)
            .Select(d => new { d.ImageDatasetId, Uri = Path.Combine(d.Volume.Uri, d.DirectoryName) })
            .Where(d => d.ImageDatasetId == selectedDataset.Id).ToListAsync(cancellationToken)).Select(d => d.Uri);

        _dbContext.ImageDatasets.Remove(selectedDataset);
        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var uri in gtDatasetUris)
        {
            _fileStorage.DeleteFolder(uri);
        }
        _fileStorage.DeleteFolder(Path.Combine(selectedDataset.Volume.Uri, selectedDataset.DirectoryName));

        return _mapper.From(selectedDataset).AdaptToType<ImageDatasetDto>();
    }
}