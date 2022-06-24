using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ClassCodeReferenceImageEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassCodeReferenceImage;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.AddClassCodeReferenceImage;

public class AddClassCodeReferenceImageHandler : DatasetHandlerBase, IRequestHandler<AddClassCodeReferenceImageCommand, ClassCodeReferenceImage>
{
    public AddClassCodeReferenceImageHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<AddClassCodeReferenceImageHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ClassCodeReferenceImage> Handle(AddClassCodeReferenceImageCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var classCodeReferenceImage = _mapper.From(request).AdaptToType<ClassCodeReferenceImageEntity>();

        var classCode =
            await _dbContext.ClassCodes.Include(c => c.ClassCodeSet.Volume)
                .Select(c => new {c.Id, c.ClassCodeSetId, Uri = Path.Combine(c.ClassCodeSet.Volume.Uri, c.ClassCodeSet.DirectoryName)})
                .SingleOrDefaultAsync(c => c.Id == classCodeReferenceImage.ClassCodeId,
                    cancellationToken: cancellationToken) ??
            throw new InvalidOperationException($"ClassCode Id={classCodeReferenceImage.ClassCodeId}");

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            GenerateLockId<ClassCodeSet>(classCode.ClassCodeSetId), cancellationToken: cancellationToken);

        var datasetUri = classCode.Uri;

        await _fileStorage.SaveFileAsync(
            datasetUri, classCodeReferenceImage.Filename, classCodeReferenceImage.Buffer!, cancellationToken);

        try
        {
            await _dbContext.ClassCodeReferenceImages.AddAsync(classCodeReferenceImage, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.DeleteFilesAsync(
                datasetUri, classCodeReferenceImage.Filename, cancellationToken);
            throw;
        }

        return _mapper.From(classCodeReferenceImage).AdaptToType<ClassCodeReferenceImage>();
    }
}