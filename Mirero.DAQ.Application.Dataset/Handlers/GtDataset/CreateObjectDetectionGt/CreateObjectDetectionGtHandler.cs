using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ObjectDetectionGtEntity = Mirero.DAQ.Domain.Dataset.Entities.ObjectDetectionGt;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateObjectDetectionGt;

public class CreateObjectDetectionGtHandler : GtDatasetHandler, IRequestHandler<CreateObjectDetectionGtCommand, ObjectDetectionGt>
{
    private readonly RequesterContext _requesterContext;

    public CreateObjectDetectionGtHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<CreateObjectDetectionGtHandler> logger,
        IFileStorage fileStorage, IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<ObjectDetectionGt> Handle(CreateObjectDetectionGtCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var objectDetectionGt = _mapper.From(request).AdaptToType<ObjectDetectionGtEntity>();

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        var gtDataset =
            await _dbContext.GtDatasets.Include(d => d.Volume)
                .SingleOrDefaultAsync(d => d.Id == objectDetectionGt.DatasetId, cancellationToken) ??
            throw new NullReferenceException();

        await using var @lock =
            await _AcquireLockByGtDatasetAsync(gtDataset, request.LockTimeoutSec, true, cancellationToken);

        if (objectDetectionGt.Buffer == null)
        {
            throw new ArgumentNullException();
        }

        var datasetUri = Path.Combine(gtDataset.Volume.Uri, gtDataset.DirectoryName);

        await _fileStorage.SaveFileAsync(datasetUri,
            objectDetectionGt.Filename, objectDetectionGt.Buffer, cancellationToken);

        try
        {
            await _dbContext.ObjectDetectionGts.AddAsync(objectDetectionGt, cancellationToken);
            gtDataset.CreateUser = _requesterContext.UserName;
            gtDataset.UpdateUser = _requesterContext.UserName;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.DeleteFileAsync(datasetUri, objectDetectionGt.Filename, cancellationToken);
            throw;
        }

        return _mapper.From(objectDetectionGt).AdaptToType<ObjectDetectionGt>();
    }
}
