using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ModelVersionEntity = Mirero.DAQ.Domain.Inference.Entities.ModelVersion;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.CreateModelVersion;

public class CreateModelVersionHandler : IRequestHandler<CreateModelVersionCommand, ModelVersion>
{
    private readonly RequesterContext _requesterContext;
    private readonly ILogger<CreateModelVersionHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateModelVersionHandler(ILogger<CreateModelVersionHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IMapper mapper,
        RequesterContext requesterContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }

        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _requesterContext = requesterContext;
    }

    public async Task<ModelVersion> Handle(CreateModelVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var modelVersion = _mapper.From(request).AdaptToType<ModelVersionEntity>();

        //var uri = (await _dbContext.Models.Include(m => m.Volume)
        //               .Select(m => new
        //               {
        //                   m.Id, Uri = Path.Combine(
        //                       m.Volume.Uri,
        //                       m.ModelName,
        //                       modelVersion.Version,
        //                       modelVersion.Filename)
        //               })
        //               .SingleOrDefaultAsync(m => m.Id == modelVersion.ModelId, cancellationToken)
        //           ?? throw new NotImplementedException()).Uri;

        await _dbContext.ModelVersions.AddAsync(modelVersion, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(modelVersion).AdaptToType<ModelVersion>();
    }
}