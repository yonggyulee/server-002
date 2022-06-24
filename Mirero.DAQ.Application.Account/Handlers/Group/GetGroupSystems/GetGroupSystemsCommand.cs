using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.GetGroupSystems
{
    public class GetGroupSystemsCommand : IRequest<GetGroupSystemsResponse>
    {
        public GetGroupSystemsRequest Request { get; set; }

        public GetGroupSystemsCommand(GetGroupSystemsRequest request)
        {
            Request = request;
        }
    }
}
