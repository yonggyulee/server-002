using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.User.ListUsers
{
    public class ListUsersCommand : IRequest<ListUsersResponse>
    {
        public ListUsersRequest Request { get; set; }

        public ListUsersCommand(ListUsersRequest request)
        {
            Request = request;
        }
    }
}
