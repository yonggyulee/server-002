using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.CreateModelVersion;

public class CreateModelVersionCommand : IRequest<ModelVersion>
{
    public CreateModelVersionRequest Request { get; set; }

    public CreateModelVersionCommand(CreateModelVersionRequest request)
    {
        Request = request;
    }
}