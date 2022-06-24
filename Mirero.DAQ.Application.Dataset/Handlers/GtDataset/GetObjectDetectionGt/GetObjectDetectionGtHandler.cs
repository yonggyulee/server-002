using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetObjectDetectionGt;

public class GetObjectDetectionGtHandler : GtDatasetHandler, IRequestHandler<GetObjectDetectionGtCommand, ObjectDetectionGt>
{
    public GetObjectDetectionGtHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetObjectDetectionGtHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }
    public async Task<ObjectDetectionGt> Handle(GetObjectDetectionGtCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var objectDetectionGt = await _dbContext.ObjectDetectionGts
                                    .Include(od => od.GtDataset.Volume)
                                    .SingleOrDefaultAsync(
                                        od => od.Id == request.ObjectDetectionGtId,
                                        cancellationToken)
                                ?? throw new NotImplementedException();

        await using var @lock = await _AcquireLockByGtDatasetAsync(objectDetectionGt.GtDataset, request.LockTimeoutSec,
            false, cancellationToken);

        objectDetectionGt.Buffer =
            await _fileStorage.GetFileBufferAsync(
                Path.Combine(objectDetectionGt.GtDataset.Volume.Uri, objectDetectionGt.GtDataset.DirectoryName),
                objectDetectionGt.Filename, cancellationToken);

        return _mapper.From(objectDetectionGt).AdaptToType<ObjectDetectionGt>();
    }
}