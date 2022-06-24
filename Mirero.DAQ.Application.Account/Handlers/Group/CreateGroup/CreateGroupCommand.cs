using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.CreateGroup;

public sealed class CreateGroupCommand : IRequest<Domain.Account.Protos.V1.Group>
{
    public CreateGroupRequest Request { get; set; }

    public CreateGroupCommand(CreateGroupRequest request)
    {
        Request = request;
    }
}
