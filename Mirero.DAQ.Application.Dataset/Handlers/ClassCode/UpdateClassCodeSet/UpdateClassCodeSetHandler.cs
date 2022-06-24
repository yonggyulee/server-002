using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.UpdateClassCodeSet;

public class UpdateClassCodeSetHandler : DatasetHandlerBase, IRequestHandler<UpdateClassCodeSetCommand, ClassCodeSet>
{
    private RequesterContext _requesterContext;

    public UpdateClassCodeSetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<UpdateClassCodeSetHandler> logger, IFileStorage fileStorage,
        IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<ClassCodeSet> Handle(UpdateClassCodeSetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (_requesterContext.UserName is null) 
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");

        var selectedClassCodeSet =
            await _dbContext.ClassCodeSets.Include(c => c.Volume)
                .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken) ??
            throw new InvalidOperationException($"ClassCodeSet Id={request.Id}");

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            GenerateLockId<ClassCodeSet>(selectedClassCodeSet.Id),
            request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var isChangedVolume = request.VolumeId != selectedClassCodeSet.VolumeId;

        var isChangedUri = isChangedVolume || request.DirectoryName != selectedClassCodeSet.DirectoryName;

        var (originUri, newUri) = ("", "");

        if (isChangedUri)
        {
            originUri = Path.Combine(selectedClassCodeSet.Volume.Uri, selectedClassCodeSet.DirectoryName);

            newUri = isChangedVolume
                ? Path.Combine((await _dbContext.Volumes.FindAsync(
                                    new object?[] {request.VolumeId},
                                    cancellationToken: cancellationToken)
                                ?? throw new InvalidOperationException($"Volue Id={request.VolumeId}")).Uri,
                    request.DirectoryName)
                : Path.Combine(selectedClassCodeSet.Volume.Uri, request.DirectoryName);
            _fileStorage.MoveFolder(originUri, newUri);
        }

        try
        {
            _mapper.From(request).AdaptTo(selectedClassCodeSet);
            selectedClassCodeSet.UpdateUser = _requesterContext.UserName;
            //selectedClassCodeSet.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            if (isChangedUri)
            {
                _fileStorage.MoveFolder(newUri, originUri);
            }

            throw;
        }

        return _mapper.From(selectedClassCodeSet).AdaptToType<ClassCodeSet>();
    }
}
