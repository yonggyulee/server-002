using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ImageDatasetEntity = Mirero.DAQ.Domain.Dataset.Entities.ImageDataset;
using ImageDatasetDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ImageDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.CreateImageDataset;

public class CreateImageDatasetHandler : DatasetHandlerBase, IRequestHandler<CreateImageDatasetCommand, Domain.Dataset.Protos.V1.ImageDataset>
{
    private readonly RequesterContext _requesterContext;

    public CreateImageDatasetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<CreateImageDatasetHandler> logger, IFileStorage fileStorage,
        IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<ImageDatasetDto> Handle(CreateImageDatasetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var imageDataset = _mapper.From(request).AdaptToType<ImageDatasetEntity>();

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        imageDataset.CreateUser = _requesterContext.UserName;
        imageDataset.UpdateUser = _requesterContext.UserName;

        var volumeUri = (await _dbContext.Volumes.FindAsync(
                             new object?[] { imageDataset.VolumeId },
                             cancellationToken: cancellationToken)
                         ?? throw new NotImplementedException()).Uri;

        var uri = Path.Combine(volumeUri, imageDataset.DirectoryName);

        await _fileStorage.CreateFolderAsync(uri, cancellationToken);

        try
        {
            //imageDataset.CreateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //imageDataset.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            await _dbContext.ImageDatasets.AddAsync(imageDataset, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            _fileStorage.DeleteFolder(imageDataset.DirectoryName);
            throw;
        }

        return _mapper.From(imageDataset).AdaptToType<ImageDatasetDto>();
    }
}
