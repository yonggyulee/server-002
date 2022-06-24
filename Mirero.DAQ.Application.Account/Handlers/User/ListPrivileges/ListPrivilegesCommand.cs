using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.User.ListPrivileges
{
    public class ListPrivilegesCommand : IRequest<ListPrivilegesResponse>
    {
        public ListPrivilegesRequest Request { get; set; }

        public ListPrivilegesCommand(ListPrivilegesRequest request)
        {
            Request = request;
        }
    }
}
