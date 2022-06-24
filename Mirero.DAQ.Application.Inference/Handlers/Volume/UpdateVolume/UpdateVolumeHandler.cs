using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using VolumeDto = Mirero.DAQ.Domain.Inference.Protos.V1.Volume;

namespace Mirero.DAQ.Application.Inference.Handlers.Volume.UpdateVolume;

public class UpdateVolumeHandler : IRequestHandler<UpdateVolumeCommand, VolumeDto>
{
    private readonly ILogger<UpdateVolumeHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;
    private readonly ILockProvider _lockProvider;
    
    public UpdateVolumeHandler(ILogger<UpdateVolumeHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IFileStorage fileStorage, IMapper mapper,
        IPostgresLockProviderFactory lockProviderFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }

        _dbContext = dbContextFactory.CreateDbContext();
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        if (lockProviderFactory == null)
        {
            throw new ArgumentNullException(nameof(lockProviderFactory));
        }

        _lockProvider = lockProviderFactory.CreateLockProvider(_dbContext);
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