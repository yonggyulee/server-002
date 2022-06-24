using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Gds;
using ServerDto = Mirero.DAQ.Domain.Gds.Protos.V1.Server;
using ServerEntity = Mirero.DAQ.Domain.Gds.Entities.Server;

namespace Mirero.DAQ.Application.Gds.Handlers.Server.UpdateServer;

public class UpdateServerHandler : IRequestHandler<UpdateServerCommand, Domain.Gds.Protos.V1.Server>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly GdsDbContext _dbContext;

    public UpdateServerHandler(ILogger<UpdateServerHandler> logger,
        IMapper mapper,
        IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<ServerDto> Handle(UpdateServerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var serverEntity = new ServerEntity
        {
            Id = request.Id,
        };
        _dbContext.Servers.Attach(serverEntity);
        _mapper.Map(request, serverEntity);

        var updatedCount = await _dbContext.SaveChangesAsync(cancellationToken);
        if (updatedCount == 0)
            throw new InvalidOperationException("요청하신 Id에 해당하는 Server가 존재하지 않거나, Server Update가 되지 않았습니다.");

        _dbContext.Servers.Update(serverEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(serverEntity).AdaptToType<ServerDto>();
    }
}