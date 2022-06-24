using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.User.UpdateUserPrivilege
{
    public class UpdateUserPrivilegeCommand : IRequest<UpdateUserPrivilegeResponse>
    {
        public UpdateUserPrivilegeRequest Request { get; set; }

        public UpdateUserPrivilegeCommand(UpdateUserPrivilegeRequest request)
        {
            Request = request;
        }
    }
}
