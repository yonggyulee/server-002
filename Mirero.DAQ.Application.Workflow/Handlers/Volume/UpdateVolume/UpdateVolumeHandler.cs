using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Storage;
using VolumeEntity = Mirero.DAQ.Domain.Workflow.Entities.Volume;

namespace Mirero.DAQ.Application.Workflow.Handlers.Volume.UpdateVolume;

public class UpdateVolumeHandler : IRequestHandler<UpdateVolumeCommand, Domain.Workflow.Protos.V1.Volume>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IFileStorage _fileStorage;

    public UpdateVolumeHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper
            , IFileStorage fileStorage)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
    }

    public async Task<Domain.Workflow.Protos.V1.Volume> Handle(UpdateVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var origin = await _dbContext.Volumes.AsNoTracking().SingleOrDefaultAsync(
                                 x => x.Id == request.Id,
                                 cancellationToken: cancellationToken)
                             ?? throw new NotImplementedException();

        var originUri = origin.Uri;
        var isChangedUri = request.Uri != originUri;

        if (isChangedUri)
        {
            _fileStorage.MoveFolder(originUri, request.Uri);
        }

        try
        {
            var volume = _mapper.From(request).AdaptTo(origin);
            _dbContext.Volumes.Update(volume);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return _mapper.From(volume).AdaptToType<Domain.Workflow.Protos.V1.Volume>();
        }
        catch
        {
            if (isChangedUri)
            {
                _fileStorage.MoveFolder(request.Uri, originUri);
            }

            throw;
        }
    }
}
