using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ClassCodeSetEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassCodeSet;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.CreateClassCodeSet;

public class CreateClassCodeSetHandler : DatasetHandlerBase, IRequestHandler<CreateClassCodeSetCommand, ClassCodeSet>
{
    private readonly RequesterContext _requesterContext;
    
    public CreateClassCodeSetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<CreateClassCodeSetHandler> logger, IFileStorage fileStorage,
        IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
        _requesterContext = requesterContext ?? throw new ArgumentNullException(nameof(requesterContext));
    }

    public async Task<ClassCodeSet> Handle(CreateClassCodeSetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var classCodeSet = _mapper.From(request).AdaptToType<ClassCodeSetEntity>();

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");

        classCodeSet.CreateUser = _requesterContext.UserName;
        classCodeSet.UpdateUser = _requesterContext.UserName;

        var volumeUri = (await _dbContext.Volumes.FindAsync(
                             new object?[] {classCodeSet.VolumeId},
                             cancellationToken) ??
                         throw new InvalidOperationException($"Volume Id={classCodeSet.VolumeId}")).Uri;

        var uri = Path.Combine(volumeUri, classCodeSet.DirectoryName);

        await _fileStorage.CreateFolderAsync(uri, cancellationToken);

        try
        {
            //classCodeSet.CreateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //classCodeSet.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _dbContext.ClassCodeSets.AddAsync(classCodeSet, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            _fileStorage.DeleteFolder(uri);
            throw;
        }

        return _mapper.From(classCodeSet).AdaptToType<ClassCodeSet>();
    }
}
