using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ServerDto = Mirero.DAQ.Domain.Inference.Protos.V1.Server;

namespace Mirero.DAQ.Application.Inference.Handlers.Server.DeleteServer;

public class DeleteServerHandler : IRequestHandler<DeleteServerCommand, ServerDto>
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

    public async Task<ServerDto> Handle(DeleteServerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var selectedServer = await _dbContext.Servers.FindAsync(
                                 new object?[] { request.ServerId },
                                 cancellationToken: cancellationToken)
                             ?? throw new NotImplementedException();

        _dbContext.Servers.Remove(selectedServer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(selectedServer).AdaptToType<ServerDto>();
    }
}