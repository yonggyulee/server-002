using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Constants;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using PrivilegeDto = Mirero.DAQ.Domain.Account.Protos.V1.Privilege;

namespace Mirero.DAQ.Application.Account.Handlers.User.ListPrivileges
{
    public class ListPrivilegesHandler : IRequestHandler<ListPrivilegesCommand, ListPrivilegesResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ListPrivilegesHandler(ILogger<ListPrivilegesHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ListPrivilegesResponse> Handle(ListPrivilegesCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var privileges = typeof(PrivilegeId).GetFields().Select(p => p.Name).ToList();
            var privilegeDtos = privileges.Select(p => { return _mapper.From((p, p)).AdaptToType<PrivilegeDto>(); });

            return _mapper.From((request, privilegeDtos, privileges.Count)).AdaptToType<ListPrivilegesResponse>();
        }
    }
}
