using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using VolumeDto = Mirero.DAQ.Domain.Dataset.Protos.V1.Volume;

namespace Mirero.DAQ.Application.Dataset.Handlers.Volume.DeleteVolume;

public class DeleteVolumeHandler : DatasetHandlerBase, IRequestHandler<DeleteVolumeCommand, VolumeDto>
{
    public DeleteVolumeHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteVolumeHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<VolumeDto> Handle(DeleteVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var selectedVolume = await _dbContext.Volumes.FindAsync(
                                 new object?[] { request.VolumeId },
                                 cancellationToken: cancellationToken)
                             ?? throw new NotImplementedException();

        _dbContext.Volumes.Remove(selectedVolume);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _fileStorage.DeleteFolder(selectedVolume.Uri);

        return _mapper.From(selectedVolume).AdaptToType<VolumeDto>();
    }
}