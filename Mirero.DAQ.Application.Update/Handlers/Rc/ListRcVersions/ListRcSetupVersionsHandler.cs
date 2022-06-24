using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Update.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Update;
using RcSetupVersion = Mirero.DAQ.Domain.Update.Entity.RcSetupVersion;
using RcSetupVersionDto = Mirero.DAQ.Domain.Update.Protos.V1.RcSetupVersion;

namespace Mirero.DAQ.Application.Update.Handlers.Rc.ListRcVersions;

public sealed class
    ListRcSetupVersionsHandler : IRequestHandler<ListRcSetupVersionsCommand, ListRcSetupVersionsResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly UpdateDbContextInmemory _dbContext;

    public ListRcSetupVersionsHandler(ILogger<ListRcSetupVersionsHandler> logger, IMapper mapper,
        IConfiguration configuration,
        IDbContextFactory<UpdateDbContextInmemory> dbContextFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<ListRcSetupVersionsResponse> Handle(ListRcSetupVersionsCommand setupVersionsCommand,
        CancellationToken cancellationToken)
    {
        var rcSetupVersionDtos = await _dbContext.RcSetupVersions
            .Select(rsv => _mapper.From(rsv).AdaptToType<RcSetupVersionDto>())
            .ToListAsync(cancellationToken);

        return _mapper.From((setupVersionsCommand.Request, rcSetupVersionDtos, rcSetupVersionDtos.Count))
            .AdaptToType<ListRcSetupVersionsResponse>();
    }
}