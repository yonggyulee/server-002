using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.ListGroups
{
    public class ListGroupsCommand : IRequest<ListGroupsResponse>
    {
        public ListGroupsRequest Request { get; set; }

        public ListGroupsCommand(ListGroupsRequest request)
        {
            Request = request;
        }
    }
}
