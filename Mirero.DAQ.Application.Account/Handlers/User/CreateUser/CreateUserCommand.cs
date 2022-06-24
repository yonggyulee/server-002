using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.User.CreateUser
{
    public class CreateUserCommand : IRequest<Domain.Account.Protos.V1.User>
    {
        public CreateUserRequest Request { get; set; }

        public CreateUserCommand(CreateUserRequest request)
        {
            Request = request;
        }
    }
}
