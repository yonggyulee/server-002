using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.UpdateGroup
{
    public class UpdateGroupCommand : IRequest<Mirero.DAQ.Domain.Account.Protos.V1.Group>
    {
        public UpdateGroupRequest Request { get; set; }

        public UpdateGroupCommand(UpdateGroupRequest request)
        {
            Request = request;
        }
    }
}
