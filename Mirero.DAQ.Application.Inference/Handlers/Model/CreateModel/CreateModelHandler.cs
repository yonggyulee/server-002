using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ModelEntity = Mirero.DAQ.Domain.Inference.Entities.Model;
using ModelDto = Mirero.DAQ.Domain.Inference.Protos.V1.Model;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.CreateModel;

public class CreateModelHandler : IRequestHandler<CreateModelCommand, ModelDto>
{
    private readonly ILogger<CreateModelHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;
    private readonly IUriGenerator _uriGenerator;

    public CreateModelHandler(ILogger<CreateModelHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IFileStorage fileStorage,
        IMapper mapper, IUriGenerator uriGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }
        _dbContext = dbContextFactory.CreateDbContext();
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
    }

    public async Task<ModelDto> Handle(CreateModelCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var model = _mapper.From(request).AdaptToType<ModelEntity>();

        var uri = (await _dbContext.Volumes.Select(
                           v => new { v.Id, Uri = _uriGenerator.GetModelUri(v.Uri, model.ModelName, false)})
                       .SingleOrDefaultAsync(
                           v => v.Id == model.VolumeId, cancellationToken)
                   ?? throw new NotImplementedException()).Uri;

        await _fileStorage.CreateFolderAsync(uri, cancellationToken);

        try
        {
            await _dbContext.Models.AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            _fileStorage.DeleteFolder(uri);
            throw;
        }

        return _mapper.From(model).AdaptToType<ModelDto>();
    }
}