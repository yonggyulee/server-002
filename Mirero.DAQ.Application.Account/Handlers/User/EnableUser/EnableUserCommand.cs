using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.User.EnableUser
{
    public class EnableUserCommand : IRequest<Empty>
    {
        public EnableUserRequest Request { get; set; }

        public EnableUserCommand(EnableUserRequest request)
        {
            Request = request;
        }
    }
}
