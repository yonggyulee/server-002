using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.GetDefaultModelVersion;

public class GetDefaultModelVersionHandler : IRequestHandler<GetDefaultModelVersionCommand, ModelVersion>
{
    private readonly ILogger<GetDefaultModelVersionHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetDefaultModelVersionHandler(ILogger<GetDefaultModelVersionHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }
        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ModelVersion> Handle(GetDefaultModelVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var dmv = await _dbContext.DefaultModelVersions.Include(d => d.ModelVersion)
                      .SingleOrDefaultAsync(d => d.ModelId == request.ModelId, cancellationToken) ??
                  throw new NullReferenceException();
        return _mapper.From(dmv.ModelVersion).AdaptToType<ModelVersion>();
    }
}