using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Gds;
using Mirero.DAQ.Infrastructure.Storage;
using VolumeDto = Mirero.DAQ.Domain.Gds.Protos.V1.Volume;
using VolumeEntity = Mirero.DAQ.Domain.Gds.Entities.Volume;

namespace Mirero.DAQ.Application.Gds.Handlers.Volume.CreateVolume;

public class CreateVolumeHandler : IRequestHandler<CreateVolumeCommand, VolumeDto>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IFileStorage _fileStorage;
    private readonly GdsDbContext _dbContext;

    public CreateVolumeHandler(ILogger<CreateVolumeHandler> logger,
        IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory, 
        IFileStorage fileStorage, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<VolumeDto> Handle(CreateVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var volume = _mapper.From(request).AdaptToType<VolumeEntity>();
        await _fileStorage.CreateFolderAsync(volume.Uri, cancellationToken);

        await _dbContext.Volumes.AddAsync(volume, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(volume).AdaptToType<VolumeDto>();
    }
}