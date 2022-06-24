using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Gds;
using Mirero.DAQ.Infrastructure.Storage;
using VolumeDto = Mirero.DAQ.Domain.Gds.Protos.V1.Volume;

namespace Mirero.DAQ.Application.Gds.Handlers.Volume.UpdateVolume;

public class UpdateVolumeHandler : IRequestHandler<UpdateVolumeCommand, VolumeDto>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IFileStorage _fileStorage;
    private readonly GdsDbContext _dbContext;
    public UpdateVolumeHandler(ILogger<UpdateVolumeHandler> logger,
        IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory,
        IFileStorage fileStorage, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<VolumeDto> Handle(UpdateVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var selectedVolume = await _dbContext.Volumes.FindAsync(
                                 new object?[] { request.Id },
                                 cancellationToken: cancellationToken) ?? throw new NotImplementedException();

        var originUri = selectedVolume.Uri;
        var isChangedUri = request.Uri != originUri;
   
        if (isChangedUri)
        {
            _fileStorage.MoveFolder(originUri, request.Uri);
        }

        var folderExist = _fileStorage.FolderExists(request.Uri);
        
        if (folderExist is false)
        {
            throw new InvalidOperationException("Request Uri 위치에 Folder가 존재하지 않습니다.");
        }
     
        _mapper.From(request).AdaptTo(selectedVolume);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(selectedVolume).AdaptToType<VolumeDto>();
    }
}