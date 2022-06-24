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

namespace Mirero.DAQ.Application.Account.Handlers.User.GetRole
{
    public class GetRoleHandler : IRequestHandler<GetRoleCommand, GetRoleResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetRoleHandler(ILogger<GetRoleHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetRoleResponse> Handle(GetRoleCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var role = typeof(RoleId).GetFields().FirstOrDefault(r => r.Name == request.RoleId)?.Name
                       ?? throw new Exception($"{request.RoleId} doesn't exist.");

            var roleDto = _mapper.From((role, role)).AdaptToType<Role>();
            return _mapper.From(roleDto).AdaptToType<GetRoleResponse>();
        }
    }
}
