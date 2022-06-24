using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ClassCodeDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ClassCode;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.DeleteClassCode;

public class DeleteClassCodeHandler : ClassCodeHandler, IRequestHandler<DeleteClassCodeCommand, ClassCodeDto>
{
    private readonly RequesterContext _requesterContext;
    public DeleteClassCodeHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteClassCodeHandler> logger,
        IFileStorage fileStorage, IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<ClassCodeDto> Handle(DeleteClassCodeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");

        var selectedClassCode = await _dbContext.ClassCodes
                                    .Include(c => c.ClassCodeSet.Volume)
                                    .Include(c => c.ClassCodeReferenceImages)
                                    .SingleOrDefaultAsync(c => c.Id == request.ClassCodeId,
                                        cancellationToken: cancellationToken)
                                ?? throw new InvalidOperationException($"ClassCode Id={request.ClassCodeId}");

        var classCodeSet = selectedClassCode.ClassCodeSet;

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            GenerateLockId<ClassCodeSet>(selectedClassCode.ClassCodeSetId), request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var (deleteUris, deletedFileBuffers) =
            await DeleteImageFilesAsync(selectedClassCode);

        try
        {
            _dbContext.ClassCodes.Remove(selectedClassCode);
            classCodeSet.CreateUser = _requesterContext.UserName;
            classCodeSet.UpdateUser = _requesterContext.UserName;
            _dbContext.ClassCodeSets.Update(classCodeSet);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.SaveFilesAsync(deleteUris, deletedFileBuffers, cancellationToken);
            throw;
        }

        return _ToClassCode(selectedClassCode);
    }
}
