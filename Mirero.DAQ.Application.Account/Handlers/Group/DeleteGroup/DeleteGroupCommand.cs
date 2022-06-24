using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.DeleteGroup;

public class DeleteGroupCommand: IRequest<Empty>
{
    public DeleteGroupRequest Request { get; set; }

    public DeleteGroupCommand(DeleteGroupRequest request)
    {
        Request = request;
    }

}