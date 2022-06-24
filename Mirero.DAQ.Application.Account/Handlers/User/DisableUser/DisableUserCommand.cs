using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.User.DisableUser
{
    public class DisableUserCommand : IRequest<Empty>
    {
        public DisableUserRequest Request { get; set; }

        public DisableUserCommand(DisableUserRequest request)
        {
            Request = request;
        }
    }
}
