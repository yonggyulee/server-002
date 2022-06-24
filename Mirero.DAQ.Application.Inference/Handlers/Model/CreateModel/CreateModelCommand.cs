using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.CreateModel;

public class CreateModelCommand : IRequest<Domain.Inference.Protos.V1.Model>
{
    public CreateModelRequest Request { get; set; }

    public CreateModelCommand(CreateModelRequest request)
    {
        Request = request;
    }
}