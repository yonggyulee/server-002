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
using Mirero.DAQ.Domain.Account.Constants;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;

namespace Mirero.DAQ.Application.Account.Handlers.User.ListRoles
{
    public class ListRolesHandler : IRequestHandler<ListRolesCommand, ListRolesResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ListRolesHandler(ILogger<ListRolesHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<ListRolesResponse> Handle(ListRolesCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var roles = typeof(RoleId).GetFields().Select(r => r.Name)?.ToList()
                        ?? throw new Exception($"RoleList doesn't exist."); ;

            var roleDtos = roles.Select(r => { return _mapper.From((r, r)).AdaptToType<Role>(); });

            return _mapper.From((request, roleDtos, roles.Count)).AdaptToType<ListRolesResponse>();
        }
    }
}
