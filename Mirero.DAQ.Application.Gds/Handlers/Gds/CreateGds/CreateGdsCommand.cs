using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.CreateGds;

public class CreateGdsCommand : IRequest<CreateGdsResponse>
{
    public CreateGdsRequest Request { get; }

    public CreateGdsCommand(CreateGdsRequest request)
    {
        Request = request;
    }
}