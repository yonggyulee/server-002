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

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.CreateClassCode;

public class CreateClassCodeHandler : ClassCodeHandler, IRequestHandler<CreateClassCodeCommand, ClassCodeDto>
{
    private RequesterContext _requesterContext;

    public CreateClassCodeHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<CreateClassCodeHandler> logger, IFileStorage fileStorage,
        IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<ClassCodeDto> Handle(CreateClassCodeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var classCode = _mapper.From(request).AdaptToType<ClassCodeEntity>();

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");

        classCode.CreateUser = _requesterContext.UserName;
        classCode.UpdateUser = _requesterContext.UserName;


        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            GenerateLockId<ClassCodeSet>(classCode.ClassCodeSetId), request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var classCodeSet =
            await _dbContext.ClassCodeSets.Include(c => c.Volume)
                .SingleOrDefaultAsync(c => c.Id == classCode.ClassCodeSetId, cancellationToken: cancellationToken) ??
            throw new InvalidOperationException($"ClassCodeSet Id={classCode.ClassCodeSetId}");

        if (classCode.ClassCodeReferenceImages.Count == 0)
        {
            //classCode.CreateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //classCode.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _dbContext.ClassCodes.AddAsync(classCode, cancellationToken);
            classCodeSet.UpdateDate = classCode.UpdateDate;
            classCodeSet.UpdateUser = classCode.UpdateUser;
            _dbContext.ClassCodeSets.Update(classCodeSet);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.From(classCode).AdaptToType<ClassCodeDto>();
        }

        var classCodeSetUri = Path.Combine(classCodeSet.Volume.Uri, classCodeSet.DirectoryName);

        var filenames = await SaveImageFilesAsync(classCodeSetUri, classCode);

        try
        {
            await _dbContext.ClassCodes.AddAsync(classCode, cancellationToken);
            classCodeSet.UpdateDate = classCode.UpdateDate;
            classCodeSet.UpdateUser = classCode.UpdateUser;
            _dbContext.ClassCodeSets.Update(classCodeSet);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.DeleteFilesAsync(classCodeSetUri, filenames, cancellationToken);
            throw;
        }

        return _ToClassCode(classCode);
    }
}
