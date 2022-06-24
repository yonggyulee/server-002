using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.UpdateModelVersion;

public class UpdateModelVersionCommand : IRequest<ModelVersion>
{
    public UpdateModelVersionRequest Request { get; set; }

    public UpdateModelVersionCommand(UpdateModelVersionRequest request)
    {
        Request = request;
    }
}