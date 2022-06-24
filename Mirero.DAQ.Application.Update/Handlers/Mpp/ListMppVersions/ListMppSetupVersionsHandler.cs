using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Update.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Update;
using MppSetupVersion = Mirero.DAQ.Domain.Update.Entity.MppSetupVersion;
using MppSetupVersionDto = Mirero.DAQ.Domain.Update.Protos.V1.MppSetupVersion;

namespace Mirero.DAQ.Application.Update.Handlers.Mpp.ListMppVersions;

public sealed class ListMppSetupVersionsHandler : IRequestHandler<ListMppSetupVersionsCommand, ListMppSetupVersionsResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly UpdateDbContextInmemory _dbContext;

    public ListMppSetupVersionsHandler(ILogger<ListMppSetupVersionsHandler> logger, IMapper mapper, IConfiguration configuration,
        IDbContextFactory<UpdateDbContextInmemory> dbContextFactory, RequesterContext requesterContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<ListMppSetupVersionsResponse> Handle(ListMppSetupVersionsCommand setupVersionsCommand, CancellationToken cancellationToken)
    {
        var mppSetupVersionDtos = await _dbContext.MppSetupVersions
            .Select(msv => _mapper.From(msv).AdaptToType<MppSetupVersionDto>())
            .ToListAsync(cancellationToken);

        return _mapper.From((setupVersionsCommand.Request, mppSetupVersionDtos, mppSetupVersionDtos.Count)).AdaptToType<ListMppSetupVersionsResponse>();
    }
}