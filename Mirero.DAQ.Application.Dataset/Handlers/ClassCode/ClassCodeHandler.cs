using Google.Protobuf;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using ClassCodeEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassCode;
using ClassCodeDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ClassCode;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode;

public class ClassCodeHandler : DatasetHandlerBase
{
    protected ClassCodeHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DatasetHandlerBase> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    protected async Task<ClassCodeDto> _ToClassCodeWithThumbnailAsync(ClassCodeEntity classCode,
        CancellationToken cancellationToken)
    {
        var classCodeDto = _mapper.From(classCode).AdaptToType<ClassCodeDto>();
        var image = classCode.ClassCodeReferenceImages.FirstOrDefault();
        //classCodeDto.ThumbnailBuffer = image == null
        //    ? null
        //    : ByteString.CopyFrom(await _fileStorage.GetFileBufferAsync(
        //        Path.Combine(classCode.ClassCodeSet.Volume.Uri, classCode.ClassCodeSet.DirectoryName), image.Filename,
        //        cancellationToken));
        return classCodeDto;
    }

    protected async Task<ClassCodeDto> _ToClassCodeWithImageAsync(ClassCodeEntity classCode,
        CancellationToken cancellationToken)
    {
        classCode.ClassCodeReferenceImages =
            (await Task.WhenAll(classCode.ClassCodeReferenceImages
                .Select(async i =>
                {
                    i.Buffer = await _fileStorage.GetFileBufferAsync(
                        Path.Combine(
                            classCode.ClassCodeSet.Volume.Uri,
                            classCode.ClassCodeSet.DirectoryName),
                        i.Filename, cancellationToken);
                    return i;
                }))).ToList();
        return _ToClassCode(classCode);
    }

    protected ClassCodeDto _ToClassCode(ClassCodeEntity model)
    {
        var classCode = _mapper.From(model).AdaptToType<ClassCodeDto>();
        classCode.ClassCodeReferenceImages.AddRange(
            model.ClassCodeReferenceImages.Select(
                i => _mapper.From(i).AdaptToType<ClassCodeReferenceImage>()));
        return classCode;
    }
}