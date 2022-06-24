using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.Handlers.Model.CreateModel;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ModelDto = Mirero.DAQ.Domain.Inference.Protos.V1.Model;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.DeleteModel;

public class DeleteModelHandler : IRequestHandler<DeleteModelCommand, Empty>
{
    private readonly ILogger<DeleteModelHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly ILockProvider _lockProvider;
    private readonly IUriGenerator _uriGenerator;

    public DeleteModelHandler(ILogger<DeleteModelHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IFileStorage fileStorage,
        IPostgresLockProviderFactory lockProviderFactory, IUriGenerator uriGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }
        _dbContext = dbContextFactory.CreateDbContext();
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        if (lockProviderFactory == null)
        {
            throw new ArgumentNullException(nameof(lockProviderFactory));
        }
        _lockProvider = lockProviderFactory.CreateLockProvider(_dbContext);
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
    }

    public async Task<Empty> Handle(DeleteModelCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var selectedModel = await _dbContext.Models.Include(m => m.Volume)
                                .SingleOrDefaultAsync(
                                    m => m.Id == request.ModelId,
                                    cancellationToken: cancellationToken)
                            ?? throw new NotImplementedException();

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            _lockProvider.GenerateLockId<ModelDto>(selectedModel.Id), request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        _dbContext.Models.Remove(selectedModel);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _fileStorage.DeleteFolder(_uriGenerator.GetModelUri(selectedModel.Volume.Uri, selectedModel.ModelName));

        //return _mapper.From(selectedModel).AdaptToType<Model>();
        return new Empty();
    }
}