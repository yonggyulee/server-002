using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ServerDto = Mirero.DAQ.Domain.Inference.Protos.V1.Server;

namespace Mirero.DAQ.Application.Inference.Handlers.Server.ListServers;

public class ListServersHandler : IRequestHandler<ListServersCommand, ListServersResponse>
{
    private readonly ILogger<ListServersHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public ListServersHandler(ILogger<ListServersHandler> logger,
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

    public async Task<ListServersResponse> Handle(ListServersCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var (count, items) = await _dbContext.Servers
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter, v => _mapper.From(v).AdaptToType<ServerDto>(),
                cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListServersResponse>();
    }
}