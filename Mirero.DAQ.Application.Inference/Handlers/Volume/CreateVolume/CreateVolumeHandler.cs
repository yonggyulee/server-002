using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using VolumeDto = Mirero.DAQ.Domain.Inference.Protos.V1.Volume;
using VolumeEntity = Mirero.DAQ.Domain.Inference.Entities.Volume;

namespace Mirero.DAQ.Application.Inference.Handlers.Volume.CreateVolume;

public class CreateVolumeHandler : IRequestHandler<CreateVolumeCommand, VolumeDto>
{
    private readonly ILogger<CreateVolumeHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;

    public CreateVolumeHandler(ILogger<CreateVolumeHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IFileStorage fileStorage, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }
        _dbContext = dbContextFactory.CreateDbContext();
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<VolumeDto> Handle(CreateVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

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