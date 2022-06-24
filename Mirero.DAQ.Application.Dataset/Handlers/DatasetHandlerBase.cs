using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using SampleEntity = Mirero.DAQ.Domain.Dataset.Entities.Sample;
using ClassCodeEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassCode;

namespace Mirero.DAQ.Application.Dataset.Handlers;

public class DatasetHandlerBase
{
    protected readonly IDbContextFactory<DatasetDbContextPostgreSQL> _dbContextFactory;
    protected readonly DatasetDbContext _dbContext;
    protected readonly IPostgresLockProviderFactory _lockProviderFactory;
    protected readonly ILockProvider _lockProvider;
    protected readonly ILogger<DatasetHandlerBase> _logger;
    protected readonly IFileStorage _fileStorage;
    protected readonly IMapper _mapper;

    public DatasetHandlerBase(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DatasetHandlerBase> logger, IFileStorage fileStorage,
        IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _lockProviderFactory = lockProviderFactory;
        _logger = logger;
        _fileStorage = fileStorage;
        _mapper = mapper;

        _dbContext = _dbContextFactory.CreateDbContext();
        _lockProvider = _lockProviderFactory.CreateLockProvider(_dbContext);
    }

    protected async Task<List<string>> SaveImageFilesAsync<TModel>(string folderUri, TModel model)
    {
        var imageFilenames = new List<string>();
        var imageBuffers = new List<byte[]>();

        switch (model)
        {
            case SampleEntity sample:
                imageFilenames = sample.Images.Select(i => i.Filename).ToList();
                imageBuffers = sample.Images.Select(i => i.Buffer).ToList()!;
                break;
            case ClassCodeEntity classCode:
                imageFilenames = classCode.ClassCodeReferenceImages.Select(i => i.Filename).ToList();
                imageBuffers = classCode.ClassCodeReferenceImages.Select(i => i.Buffer).ToList()!;
                break;
        }

        await _fileStorage.SaveFilesAsync(folderUri, imageFilenames, imageBuffers);

        return imageFilenames;
    }

    protected async Task<(IEnumerable<string>, IEnumerable<byte[]>)> DeleteImageFilesAsync<TModel>(TModel model)
    {
        var deleteUris = new List<string>();

        switch (model)
        {
            case SampleEntity sample:
                var imgIds = sample.Images.Select(i => i.Id).ToList();
                var ogtUris = await _dbContext.ObjectDetectionGts.Include(gt => gt.GtDataset.Volume)
                    .Select(gt => new
                    {
                        gt.ImageId,
                        Uri = Path.Combine(gt.GtDataset.Volume.Uri, gt.GtDataset.DirectoryName, gt.Filename)
                    }).Where(gt => imgIds.Contains(gt.ImageId)).Select(gt => gt.Uri).ToListAsync();
                var sgtUris = await _dbContext.ObjectDetectionGts.Include(gt => gt.GtDataset.Volume)
                    .Select(gt => new
                    {
                        gt.ImageId,
                        Uri = Path.Combine(gt.GtDataset.Volume.Uri, gt.GtDataset.DirectoryName, gt.Filename)
                    }).Where(gt => imgIds.Contains(gt.ImageId)).Select(gt => gt.Uri).ToListAsync();
                deleteUris = sample.Images.Select(i =>
                        Path.Combine(sample.ImageDataset.Volume.Uri, sample.ImageDataset.DirectoryName, i.Filename))
                    .ToList();
                deleteUris.AddRange(ogtUris);
                deleteUris.AddRange(sgtUris);
                break;
            case ClassCodeEntity classCode:
                deleteUris = classCode.ClassCodeReferenceImages.Select(i =>
                        Path.Combine(classCode.ClassCodeSet.Volume.Uri, classCode.ClassCodeSet.DirectoryName,
                            i.Filename))
                    .ToList();
                break;
        }

        var deletedFileBuffers =
            await _fileStorage.DeleteFilesAsync(deleteUris);

        return (deleteUris, deletedFileBuffers);
    }

    protected bool IsExists<TModel, TKey>(TKey key) where TModel : class
    {
        return _dbContext.Find<TModel>(key) != null;
    }

    protected static string GenerateLockId<TEntity>(object id)
    {
        return typeof(TEntity).Name + id.ToString();
    }
}
