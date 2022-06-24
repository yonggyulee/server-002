using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.ListSystems
{
    public class ListSystemsCommand : IRequest<ListSystemsResponse>
    {
        public ListSystemsRequest Request { get; set; }

        public ListSystemsCommand(ListSystemsRequest request)
        {
            Request = request;
        }
    }
}
