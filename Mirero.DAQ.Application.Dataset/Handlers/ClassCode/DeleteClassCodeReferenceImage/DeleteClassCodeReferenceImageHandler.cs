using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.DeleteClassCodeReferenceImage;

public class DeleteClassCodeReferenceImageHandler : DatasetHandlerBase, IRequestHandler<DeleteClassCodeReferenceImageCommand, ClassCodeReferenceImage>
{
    public DeleteClassCodeReferenceImageHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteClassCodeReferenceImageHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }
    public async Task<ClassCodeReferenceImage> Handle(DeleteClassCodeReferenceImageCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var selectedClassCodeReferenceImage = await _dbContext.ClassCodeReferenceImages
                                                  .Include(s => s.ClassCode.ClassCodeSet.Volume)
                                                  .SingleOrDefaultAsync(
                                                      s => s.Id == request.ClassCodeReferenceImageId,
                                                      cancellationToken: cancellationToken)
                                              ?? throw new InvalidOperationException(
                                                  $"ClassCodeReferenceImage Id={request.ClassCodeReferenceImageId}");

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            GenerateLockId<ClassCodeSet>(selectedClassCodeReferenceImage.ClassCode.ClassCodeSetId),
            request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var setUri = Path.Combine(
            selectedClassCodeReferenceImage.ClassCode.ClassCodeSet.Volume.Uri,
            selectedClassCodeReferenceImage.ClassCode.ClassCodeSet.DirectoryName);

        var deletedFileBuffer =
            await _fileStorage.DeleteFileAsync(setUri, selectedClassCodeReferenceImage.Filename, cancellationToken);

        try
        {
            _dbContext.ClassCodeReferenceImages.Remove(selectedClassCodeReferenceImage);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.SaveFileAsync(
                setUri, selectedClassCodeReferenceImage.Filename, deletedFileBuffer, cancellationToken);
            throw;
        }

        return _mapper.From(selectedClassCodeReferenceImage).AdaptToType<ClassCodeReferenceImage>();
    }
}