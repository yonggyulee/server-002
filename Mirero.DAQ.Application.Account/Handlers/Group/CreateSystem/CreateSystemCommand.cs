using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.CreateSystem;

public class CreateSystemCommand : IRequest<Domain.Account.Protos.V1.System>
{
    public CreateSystemRequest Request { get; set; }
    public CreateSystemCommand(CreateSystemRequest request)
    {
        Request = request;
    }
}