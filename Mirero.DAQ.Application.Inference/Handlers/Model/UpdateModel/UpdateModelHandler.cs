using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ModelDto = Mirero.DAQ.Domain.Inference.Protos.V1.Model;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.UpdateModel;

public class UpdateModelHandler : IRequestHandler<UpdateModelCommand, ModelDto>
{
    private readonly ILogger<UpdateModelHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;
    private readonly ILockProvider _lockProvider;
    private readonly IUriGenerator _uriGenerator;

    public UpdateModelHandler(ILogger<UpdateModelHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IFileStorage fileStorage,
        IPostgresLockProviderFactory lockProviderFactory, IMapper mapper, IUriGenerator uriGenerator)
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
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
    }

    public async Task<ModelDto> Handle(UpdateModelCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var selectedModel = await _dbContext.Models.Include(m => m.Volume)
                                .SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
                            ?? throw new NotImplementedException();

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            _lockProvider.GenerateLockId<ModelDto>(selectedModel.Id), request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var isChangedVolume = selectedModel.VolumeId != request.VolumeId;

        var isChangedUri = isChangedVolume || selectedModel.ModelName != request.ModelName;

        var (originUri, newUri) = ("", "");

        if (isChangedUri)
        {
            newUri = isChangedVolume
                ? _uriGenerator.GetModelUri(
                    (await _dbContext.Volumes.FindAsync(
                         new object?[] { request.VolumeId },
                         cancellationToken)
                     ?? throw new NotImplementedException()).Uri, request.ModelName)
                : _uriGenerator.GetModelUri(selectedModel.Volume.Uri, request.ModelName);

            originUri = _uriGenerator.GetModelUri(selectedModel.Volume.Uri, selectedModel.ModelName);

            _fileStorage.MoveFolder(originUri, newUri);
        }

        try
        {
            _mapper.From(request).AdaptTo(selectedModel);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            if (isChangedUri)
            {
                _fileStorage.MoveFolder(newUri, originUri);
            }

            throw;
        }

        return _mapper.From(selectedModel).AdaptToType<ModelDto>();
    }
}