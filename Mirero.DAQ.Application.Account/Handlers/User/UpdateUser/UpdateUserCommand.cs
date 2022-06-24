using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.User.UpdateUser
{
    public class UpdateUserCommand : IRequest<Domain.Account.Protos.V1.User>
    {
        public UpdateUserRequest Request { get; set; }

        public UpdateUserCommand(UpdateUserRequest request)
        {
            Request = request;
        }
    }
}
