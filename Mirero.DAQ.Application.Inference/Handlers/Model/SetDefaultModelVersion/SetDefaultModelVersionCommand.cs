using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.SetDefaultModelVersion;

public class SetDefaultModelVersionCommand : IRequest<Empty>
{
    public SetDefaultModelVersionRequest Request { get; set; }

    public SetDefaultModelVersionCommand(SetDefaultModelVersionRequest request)
    {
        Request = request;
    }
}