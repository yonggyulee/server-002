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
using ObjectDetectionGtDatasetEntity = Mirero.DAQ.Domain.Dataset.Entities.ObjectDetectionGtDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateObjectDetectionGtDataset;

public class CreateObjectDetectionGtDatasetHandler : GtDatasetHandler, IRequestHandler<CreateObjectDetectionGtDatasetCommand, ObjectDetectionGtDataset>
{
    private RequesterContext _requesterContext;

    public CreateObjectDetectionGtDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<CreateObjectDetectionGtDatasetHandler> logger,
        IFileStorage fileStorage, IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<ObjectDetectionGtDataset> Handle(CreateObjectDetectionGtDatasetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        // if (IsExistsByAlternativeKey<GtDatasetEntity>("title", request.Title))
        // {
        //     throw new NotImplementedException($"{typeof(ObjectDetectionGtDatasetEntity)} Title already exists.");
        // }

        var dataset = _mapper.From(request).AdaptToType<ObjectDetectionGtDatasetEntity>();

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        dataset.CreateUser = _requesterContext.UserName;
        dataset.UpdateUser = _requesterContext.UserName;

        var volumeUri = (await _dbContext.Volumes.FindAsync(
                             new object?[] { request.VolumeId },
                             cancellationToken: cancellationToken)
                         ?? throw new NotImplementedException()).Uri;

        var uri = Path.Combine(volumeUri, dataset.DirectoryName);

        await _fileStorage.CreateFolderAsync(uri, cancellationToken);

        try
        {
            //dataset.CreateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //dataset.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _dbContext.ObjectDetectionGtDatasets.AddAsync(dataset, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
    {
            _fileStorage.DeleteFolder(uri);
            throw;
        }

        return _mapper.From(dataset).AdaptToType<ObjectDetectionGtDataset>();
    }
}
