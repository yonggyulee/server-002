using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.User.ResetUserPrivilege
{
    public class ResetUserPrivilegeCommand : IRequest<Empty>
    {
        public ResetUserPrivilegeRequest Request { get; set; }

        public ResetUserPrivilegeCommand(ResetUserPrivilegeRequest request)
        {
            Request = request;
        }
    }
}
