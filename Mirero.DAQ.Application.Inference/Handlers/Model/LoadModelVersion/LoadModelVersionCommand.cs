using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.RegisterModelVersion;

public class LoadModelVersionCommand : IRequest<LoadModelResponse>
{
    public LoadModelRequest Request { get; set; }

    public LoadModelVersionCommand(LoadModelRequest request)
    {
        Request = request;
    }
}