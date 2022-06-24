using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Gds;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.ListGdses;

public class ListGdsesHandler : IRequestHandler<ListGdsesCommand, ListGdsesResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly GdsDbContext _dbContext;
    public ListGdsesHandler(ILogger<ListGdsesHandler> logger,
         IMapper mapper,
         IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<ListGdsesResponse> Handle(ListGdsesCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.Gds
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter,
                v => _mapper.From(v).AdaptToType<Domain.Gds.Protos.V1.Gds>(),
                cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListGdsesResponse>();
    }
}