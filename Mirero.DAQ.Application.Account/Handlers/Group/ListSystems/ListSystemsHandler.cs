using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Infrastructure.Database.Account;
using SystemDto = Mirero.DAQ.Domain.Account.Protos.V1.System;

namespace Mirero.DAQ.Application.Account.Handlers.Group.ListSystems
{
    public class ListSystemsHandler : IRequestHandler<ListSystemsCommand, ListSystemsResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public ListSystemsHandler(ILogger<ListSystemsHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<ListSystemsResponse> Handle(ListSystemsCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var queryResult = _dbContext.Systems
                .OrderBy(s => s.Id)
                .AsNoTracking()
                .AsPagedResult(request.QueryParameter);

            var systemDtos = queryResult.Items.Select(s => { return _mapper.From((s.Id, s.Title)).AdaptToType<SystemDto>(); });
            return _mapper.From((request, systemDtos, queryResult.Count)).AdaptToType<ListSystemsResponse>();
        }
    }
}
