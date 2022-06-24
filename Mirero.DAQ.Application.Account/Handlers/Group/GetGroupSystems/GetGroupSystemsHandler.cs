using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Infrastructure.Database.Account;

namespace Mirero.DAQ.Application.Account.Handlers.Group.GetGroupSystems
{
    public class GetGroupSystemsHandler : IRequestHandler<GetGroupSystemsCommand, GetGroupSystemsResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public GetGroupSystemsHandler(ILogger<GetGroupSystemsHandler> logger, IMapper mapper, 
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<GetGroupSystemsResponse> Handle(GetGroupSystemsCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var checkGroup = await _dbContext.Groups.SingleOrDefaultAsync(s => s.Id == request.GroupId, cancellationToken)
                             ?? throw new Exception($"{request.GroupId} doesn't exist.");

            var queryResult = await _dbContext.GroupSystems
                .OrderBy(gs => gs.SystemId)
                .Where(gs => gs.GroupId == request.GroupId)
                .AsNoTracking()
                .AsPagedResultAsync(request.QueryParameter, cancellationToken);

            var groupSystemDtos = queryResult.Items.Select(gs =>
            {
                return _mapper.From((gs.SystemId, gs.SystemId)).AdaptToType<Domain.Account.Protos.V1.System>();
            });

            return _mapper.From((request, groupSystemDtos, queryResult.Count)).AdaptToType<GetGroupSystemsResponse>();
        }
    }
}
