using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.DeleteModel;

public class DeleteModelCommand : IRequest<Empty>
{
    public DeleteModelRequest Request { get; set; }

    public DeleteModelCommand(DeleteModelRequest request)
    {
        Request = request;
    }
}