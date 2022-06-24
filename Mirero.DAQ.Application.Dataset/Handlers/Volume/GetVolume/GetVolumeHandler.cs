using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using VolumeDto = Mirero.DAQ.Domain.Dataset.Protos.V1.Volume;

namespace Mirero.DAQ.Application.Dataset.Handlers.Volume.GetVolume;

public class GetVolumeHandler : DatasetHandlerBase, IRequestHandler<GetVolumeCommand, VolumeDto>
{
    public GetVolumeHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetVolumeHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<VolumeDto> Handle(GetVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var volume = await _dbContext.Volumes.AsNoTracking()
                         .SingleOrDefaultAsync(
                             v => v.Id == request.VolumeId, cancellationToken: cancellationToken)
                     ?? throw new NotImplementedException();
        return _mapper.From(volume).AdaptToType<VolumeDto>();
    }
}