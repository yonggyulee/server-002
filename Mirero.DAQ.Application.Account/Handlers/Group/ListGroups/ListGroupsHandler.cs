using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Infrastructure.Database.Account;
using GroupDto = Mirero.DAQ.Domain.Account.Protos.V1.Group;

namespace Mirero.DAQ.Application.Account.Handlers.Group.ListGroups
{
    public class ListGroupsHandler : IRequestHandler<ListGroupsCommand, ListGroupsResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public ListGroupsHandler(ILogger<ListGroupsHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<ListGroupsResponse> Handle(ListGroupsCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var queryResult = await _dbContext.Groups
                .OrderBy(g => g.Id)
                .AsNoTracking()
                .AsPagedResultAsync(request.QueryParameter, cancellationToken: cancellationToken);

            var groupDtos = queryResult.Items.Select((g) => { return _mapper.From((g)).AdaptToType<GroupDto>(); });

            return _mapper.From((request, groupDtos, queryResult.Count)).AdaptToType<ListGroupsResponse>();
        }
    }
}
