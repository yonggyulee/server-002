using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.UpdateSystem
{
    public class UpdateSystemCommand : IRequest<Mirero.DAQ.Domain.Account.Protos.V1.System>
    {
        public UpdateSystemRequest Request { get; set; }

        public UpdateSystemCommand(UpdateSystemRequest request)
        {
            Request = request;
        }
    }
}
