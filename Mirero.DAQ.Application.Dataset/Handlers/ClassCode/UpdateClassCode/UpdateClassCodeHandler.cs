using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ClassCodeEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassCode;
using ClassCodeDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ClassCode;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.UpdateClassCode;

public class UpdateClassCodeHandler : ClassCodeHandler, IRequestHandler<UpdateClassCodeCommand, ClassCodeDto>
{
    private RequesterContext _requesterContext;

    public UpdateClassCodeHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<UpdateClassCodeHandler> logger, IFileStorage fileStorage,
        IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<ClassCodeDto> Handle(UpdateClassCodeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var classCode = _mapper.From(request).AdaptToType<ClassCodeEntity>();

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");

        classCode.CreateUser = _requesterContext.UserName;
        classCode.UpdateUser = _requesterContext.UserName;

        var selectedClassCode = await _dbContext.ClassCodes
                                    .Include(c => c.ClassCodeSet.Volume)
                                    .Include(c => c.ClassCodeReferenceImages)
                                    .SingleOrDefaultAsync(
                                        c => c.Id == classCode.Id,
                                        cancellationToken: cancellationToken)
                                ?? throw new InvalidOperationException($"ClassCode Id={classCode.Id}");

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            GenerateLockId<ClassCodeSet>(selectedClassCode.ClassCodeSetId), request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var setUri = Path.Combine(selectedClassCode.ClassCodeSet.Volume.Uri,
            selectedClassCode.ClassCodeSet.DirectoryName);

        var (deleteUris, deletedFileBuffers) = await DeleteImageFilesAsync(selectedClassCode);
        var newFilenames = await SaveImageFilesAsync(setUri, classCode);

        try
        {
            _mapper.From(classCode).AdaptTo(selectedClassCode);
            selectedClassCode.ClassCodeSet.UpdateDate = classCode.UpdateDate;
            selectedClassCode.ClassCodeSet.UpdateUser = classCode.UpdateUser;
            //selectedClassCode.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.DeleteFilesAsync(setUri, newFilenames, cancellationToken);
            await _fileStorage.SaveFilesAsync(deleteUris, deletedFileBuffers, cancellationToken);
            throw;
        }

        return _ToClassCode(selectedClassCode);
    }
}
