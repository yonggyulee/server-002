using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.DeleteSystem
{
    public class DeleteSystemCommand : IRequest<Empty>
    {
        public DeleteSystemRequest Request { get; set; }
        public DeleteSystemCommand(DeleteSystemRequest request)
        {
            Request = request;
        }
    }
}
