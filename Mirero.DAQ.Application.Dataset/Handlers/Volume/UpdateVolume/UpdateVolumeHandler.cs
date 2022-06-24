using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using VolumeDto = Mirero.DAQ.Domain.Dataset.Protos.V1.Volume;

namespace Mirero.DAQ.Application.Dataset.Handlers.Volume.UpdateVolume;

public class UpdateVolumeHandler : DatasetHandlerBase, IRequestHandler<UpdateVolumeCommand, VolumeDto>
{
    public UpdateVolumeHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<UpdateVolumeHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<VolumeDto> Handle(UpdateVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var selectedVolume = await _dbContext.Volumes.FindAsync(
                                 new object?[] { request.Id },
                                 cancellationToken: cancellationToken)
                             ?? throw new NotImplementedException();

        var originUri = selectedVolume.Uri;
        var isChangedUri = request.Uri != originUri;

        if (isChangedUri)
        {
            _fileStorage.MoveFolder(originUri, request.Uri);
        }

        try
        {
            _mapper.From(request).AdaptTo(selectedVolume);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            if (isChangedUri)
            {
                _fileStorage.MoveFolder(request.Uri, originUri);
            }

            throw;
        }

        return _mapper.From(selectedVolume).AdaptToType<VolumeDto>();
    }
}