using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ServerDto = Mirero.DAQ.Domain.Inference.Protos.V1.Server;
using ServerEntity = Mirero.DAQ.Domain.Inference.Entities.Server;
namespace Mirero.DAQ.Application.Inference.Handlers.Server.CreateServer;

public class CreateServerHandler : IRequestHandler<CreateServerCommand, ServerDto>
{
    private readonly ILogger<CreateServerHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;
    

    public CreateServerHandler(ILogger<CreateServerHandler> logger,
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

    public async Task<ServerDto> Handle(CreateServerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var server = _mapper.From(request).AdaptToType<ServerEntity>();

        try
        {
            await _dbContext.Servers.AddAsync(server, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            throw;
        }

        return _mapper.From(server).AdaptToType<ServerDto>();
    }
}