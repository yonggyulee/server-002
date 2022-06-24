using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.User.GetRole
{
    public class GetRoleCommand : IRequest<GetRoleResponse>
    {
        public GetRoleRequest Request { get; set; }

        public GetRoleCommand(GetRoleRequest request)
        {
            Request = request;
        }
    }
}
