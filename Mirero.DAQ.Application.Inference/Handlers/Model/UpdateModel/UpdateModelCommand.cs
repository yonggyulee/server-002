using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.UpdateModel;

public class UpdateModelCommand : IRequest<Domain.Inference.Protos.V1.Model>
{
    public UpdateModelRequest Request { get; set; }

    public UpdateModelCommand(UpdateModelRequest request)
    {
        Request = request;
    }
}