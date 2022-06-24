using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using DefaultModelVersionEntity = Mirero.DAQ.Domain.Inference.Entities.DefaultModelVersion;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.SetDefaultModelVersion;

public class SetDefaultModelVersionHandler : IRequestHandler<SetDefaultModelVersionCommand, Empty>
{
    private readonly ILogger<SetDefaultModelVersionHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public SetDefaultModelVersionHandler(ILogger<SetDefaultModelVersionHandler> logger,
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

    public async Task<Empty> Handle(SetDefaultModelVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var dmv = await _dbContext.DefaultModelVersions.SingleOrDefaultAsync(d => d.ModelId == request.ModelId,
            cancellationToken);
        if (dmv == null)
        {
            dmv = _mapper.From(request).AdaptToType<DefaultModelVersionEntity>();
            var result = await _dbContext.AddAsync(dmv, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new Empty();
        }
        if (dmv != null && !request.Change)
        {
            throw new ArgumentException("is already exists.");
        }

        _mapper.From(request).AdaptTo(dmv);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new Empty();
    }
}