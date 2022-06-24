using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteObjectDetectionGt;

public class DeleteObjectDetectionGtHandler : GtDatasetHandler, IRequestHandler<DeleteObjectDetectionGtCommand, ObjectDetectionGt>
{
    private readonly RequesterContext _requesterContext;

    public DeleteObjectDetectionGtHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteObjectDetectionGtHandler> logger,
        IFileStorage fileStorage, IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
        _requesterContext = requesterContext;
    }
    public async Task<ObjectDetectionGt> Handle(DeleteObjectDetectionGtCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        var selectedGt = await _dbContext.ObjectDetectionGts
                             .Include(s => s.GtDataset.Volume)
                             .SingleOrDefaultAsync(
                                 s =>
                                     s.Id == request.ObjectDetectionGtId,
                                 cancellationToken: cancellationToken)
                         ?? throw new NotImplementedException();

        var objectDetectionGtDataset = selectedGt.GtDataset;

        await using var @lock =
            await _AcquireLockByGtDatasetAsync(selectedGt.GtDataset, request.LockTimeoutSec, true, cancellationToken);

        var uri = Path.Combine(selectedGt.GtDataset.Volume.Uri, selectedGt.GtDataset.DirectoryName);

        var deletedFileBuffer = await _fileStorage.DeleteFileAsync(uri, selectedGt.Filename, cancellationToken);

        try
        {
            _dbContext.ObjectDetectionGts.Remove(selectedGt);
            objectDetectionGtDataset.CreateUser = _requesterContext.UserName;
            objectDetectionGtDataset.UpdateUser = _requesterContext.UserName;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.SaveFileAsync(uri, selectedGt.Filename, deletedFileBuffer, cancellationToken);
            throw;
        }

        return _mapper.From(selectedGt).AdaptToType<ObjectDetectionGt>();
    }
}
