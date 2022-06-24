using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.User.DeleteUser
{
    public class DeleteUserCommand : IRequest<Empty>
    {
        public DeleteUserRequest Request { get; set; }

        public DeleteUserCommand(DeleteUserRequest request)
        {
            Request = request;
        }
    }
}
