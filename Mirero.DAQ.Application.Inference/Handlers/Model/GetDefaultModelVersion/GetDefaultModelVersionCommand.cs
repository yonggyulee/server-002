using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.GetDefaultModelVersion;

public class GetDefaultModelVersionCommand : IRequest<ModelVersion>
{
    public GetDefaultModelVersionRequest Request { get; set; }

    public GetDefaultModelVersionCommand(GetDefaultModelVersionRequest request)
    {
        Request = request;
    }
}