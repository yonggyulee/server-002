using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Gds;
using Mirero.DAQ.Infrastructure.Storage;
using VolumeDto = Mirero.DAQ.Domain.Gds.Protos.V1.Volume;

namespace Mirero.DAQ.Application.Gds.Handlers.Volume.DeleteVolume;

public class DeleteVolumeHandler : IRequestHandler<DeleteVolumeCommand, Empty>
{
    private readonly ILogger _logger;
    private readonly IFileStorage _fileStorage;
    private readonly GdsDbContext _dbContext;
    public DeleteVolumeHandler(ILogger<DeleteVolumeHandler> logger,
        IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory, 
        IFileStorage fileStorage) 
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<Empty> Handle(DeleteVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var selectedVolume = await _dbContext.Volumes.FindAsync(
                                 new object?[] { request.VolumeId },
                                 cancellationToken: cancellationToken)
                             ?? throw new NotImplementedException();
        
        _dbContext.Volumes.Remove(selectedVolume);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _fileStorage.DeleteFolder(selectedVolume.Uri); 
        
        if (_fileStorage.FolderExists(selectedVolume.Uri))
            throw new InvalidOperationException("Volume이 제거 되지 않았습니다.");
        
        return new Empty();
    }
}