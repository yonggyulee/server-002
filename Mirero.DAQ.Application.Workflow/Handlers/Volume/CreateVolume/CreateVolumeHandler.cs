using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Storage;
using VolumeEntity = Mirero.DAQ.Domain.Workflow.Entities.Volume;


namespace Mirero.DAQ.Application.Workflow.Handlers.Volume.CreateVolume;

public class CreateVolumeHandler : IRequestHandler<CreateVolumeCommand, Domain.Workflow.Protos.V1.Volume>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IFileStorage _fileStorage;

    public CreateVolumeHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper
            , IFileStorage fileStorage) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
    }

    public async Task<Domain.Workflow.Protos.V1.Volume> Handle(CreateVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        if (IsExists<VolumeEntity, string>(request.Id))
        {
            throw new NotImplementedException($"{typeof(VolumeEntity)} Id already exists.");
        }

        var volume = _mapper.From(request).AdaptToType<VolumeEntity>();
        await _fileStorage.CreateFolderAsync(volume.Uri, cancellationToken);

        await _dbContext.Volumes.AddAsync(volume, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
       
        return _mapper.From(volume).AdaptToType<Domain.Workflow.Protos.V1.Volume>();
    }
    private bool IsExists<TModel, TKey>(TKey key) where TModel : class
    {
        return _dbContext.Find<TModel>(key) != null;
    }
}
