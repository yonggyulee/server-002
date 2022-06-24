using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ServerDto = Mirero.DAQ.Domain.Inference.Protos.V1.Server;
using ServerEntity = Mirero.DAQ.Domain.Inference.Entities.Server;

namespace Mirero.DAQ.Application.Inference.Handlers.Server.UpdateServer;

public class DeleteServerHandler : IRequestHandler<UpdateServerCommand, ServerDto>
{
    private readonly ILogger<DeleteServerHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteServerHandler(ILogger<DeleteServerHandler> logger,
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

    public async Task<ServerDto> Handle(UpdateServerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var server = _mapper.From(request).AdaptToType<ServerEntity>();

        //var selectedServer = await _dbContext.Servers.FindAsync(
        //                         new object?[] { server.Id },
        //                         cancellationToken: cancellationToken)
        //                     ?? throw new NotImplementedException();
        try
        {
            _dbContext.Servers.Update(server);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            throw;
        }

        return _mapper.From(server).AdaptToType<ServerDto>();
    }
}