using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Gds;
using ServerDto = Mirero.DAQ.Domain.Gds.Protos.V1.Server;
using ServerEntity = Mirero.DAQ.Domain.Gds.Entities.Server;

namespace Mirero.DAQ.Application.Gds.Handlers.Server.CreateServer;

public class CreateServerHandler : IRequestHandler<CreateServerCommand, Domain.Gds.Protos.V1.Server>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly GdsDbContext _dbContext;

    public CreateServerHandler(ILogger<CreateServerHandler> logger,
        IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<ServerDto> Handle(CreateServerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var server = _mapper.From(request).AdaptToType<ServerEntity>();

        await _dbContext.Servers.AddAsync(server, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(server).AdaptToType<ServerDto>();
    }
}