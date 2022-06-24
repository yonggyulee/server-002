using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ImageDatasetDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ImageDataset;
using ImageEntity = Mirero.DAQ.Domain.Dataset.Entities.Image;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.AddImage;

public class AddImageHandler : DatasetHandlerBase, IRequestHandler<AddImageCommand, Image>
{
    public AddImageHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<AddImageHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<Image> Handle(AddImageCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var image = _mapper.From(request).AdaptToType<ImageEntity>();

        await using var @lock =
            await _lockProvider.AcquireWriteLockAsync(GenerateLockId<ImageDatasetDto>(image.DatasetId),
                cancellationToken: cancellationToken);

        var datasetUri = (await _dbContext.ImageDatasets
                              .Include(d => d.Volume)
                              .Select(d => new { d.Id, Uri = Path.Combine(d.Volume.Uri, d.DirectoryName) })
                              .SingleOrDefaultAsync(d => d.Id == image.DatasetId,
                                  cancellationToken: cancellationToken)
                          ?? throw new NotImplementedException()).Uri;

        await _fileStorage.SaveFileAsync(datasetUri, image.Filename, image.Buffer!, cancellationToken);

        try
        {
            await _dbContext.Images.AddAsync(image, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.DeleteFilesAsync(datasetUri, image.Filename, cancellationToken);
            throw;
        }

        return _mapper.From(image).AdaptToType<Image>();
    }
}