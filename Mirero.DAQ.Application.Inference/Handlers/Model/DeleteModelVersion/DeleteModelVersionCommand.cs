using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.DeleteModelVersion;

public class DeleteModelVersionCommand : IRequest<Empty>
{
    public DeleteModelVersionRequest Request { get; set; }

    public DeleteModelVersionCommand(DeleteModelVersionRequest request)
    {
        Request = request;
    }
}