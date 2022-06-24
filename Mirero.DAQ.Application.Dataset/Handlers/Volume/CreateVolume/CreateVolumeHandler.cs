using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Dataset.Handlers.Volume.CreateVolume;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using VolumeEntity = Mirero.DAQ.Domain.Dataset.Entities.Volume;
using VolumeDto = Mirero.DAQ.Domain.Dataset.Protos.V1.Volume;

namespace Mirero.DAQ.Application.Dataset.Handlers.Volume.CreateVolume;

public class CreateVolumeHandler : DatasetHandlerBase, IRequestHandler<CreateVolumeCommand, VolumeDto>
{
    public CreateVolumeHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<CreateVolumeHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<VolumeDto> Handle(CreateVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (IsExists<VolumeEntity, string>(request.Id))
        {
            throw new NotImplementedException($"{typeof(VolumeEntity)} Id already exists.");
        }

        var volume = _mapper.From(request).AdaptToType<VolumeEntity>();

        await _fileStorage.CreateFolderAsync(volume.Uri, cancellationToken);

        try
        {
            await _dbContext.Volumes.AddAsync(volume, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            _fileStorage.DeleteFolder(volume.Uri);
            throw;
        }

        return _mapper.From(volume).AdaptToType<VolumeDto>();
    }
}