using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using VolumeDto = Mirero.DAQ.Domain.Inference.Protos.V1.Volume;

namespace Mirero.DAQ.Application.Inference.Handlers.Volume.DeleteVolume;

public class DeleteVolumeHandler : IRequestHandler<DeleteVolumeCommand, VolumeDto>
{
    private readonly ILogger<DeleteVolumeHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;
    private readonly ILockProvider _lockProvider;

    public DeleteVolumeHandler(ILogger<DeleteVolumeHandler> logger,
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