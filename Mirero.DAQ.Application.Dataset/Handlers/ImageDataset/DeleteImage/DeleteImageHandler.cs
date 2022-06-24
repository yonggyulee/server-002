using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ImageDatasetDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ImageDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.DeleteImage;

public class DeleteImageHandler : DatasetHandlerBase, IRequestHandler<DeleteImageCommand, Image>
{
    public DeleteImageHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteImageHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<Image> Handle(DeleteImageCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var selectedImage = await _dbContext.Images.Include(i => i.Sample.ImageDataset.Volume).SingleOrDefaultAsync(
                                s => s.Id == request.ImageId, cancellationToken: cancellationToken)
                            ?? throw new NotImplementedException();

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            GenerateLockId<ImageDatasetDto>(selectedImage.DatasetId), request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var deleteUris = new List<string>();

        var datasetUri = Path.Combine(selectedImage.Sample.ImageDataset.Volume.Uri,
            selectedImage.Sample.ImageDataset.DirectoryName, selectedImage.Filename);

        var ogtUris = (await _dbContext.ObjectDetectionGts.Include(gt => gt.GtDataset.Volume)
            .Select(gt => new
            { gt.ImageId, Uri = Path.Combine(gt.GtDataset.Volume.Uri, gt.GtDataset.DirectoryName, gt.Filename) })
            .Where(gt => gt.ImageId == selectedImage.Id).ToListAsync(cancellationToken)).Select(gt => gt.Uri).ToList();

        var sgtUris = (await _dbContext.SegmentationGts.Include(gt => gt.GtDataset.Volume)
            .Select(gt => new
            { gt.ImageId, Uri = Path.Combine(gt.GtDataset.Volume.Uri, gt.GtDataset.DirectoryName, gt.Filename) })
            .Where(gt => gt.ImageId == selectedImage.Id).ToListAsync(cancellationToken)).Select(gt => gt.Uri);

        deleteUris.Add(datasetUri);
        deleteUris.AddRange(ogtUris);
        deleteUris.AddRange(sgtUris);

        var deletedFileBuffer = await _fileStorage.DeleteFilesAsync(deleteUris, cancellationToken);

        try
        {
            _dbContext.Images.Remove(selectedImage);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.SaveFilesAsync(deleteUris, deletedFileBuffer, cancellationToken);
            throw;
        }

        return _mapper.From(selectedImage).AdaptToType<Image>();
    }
}