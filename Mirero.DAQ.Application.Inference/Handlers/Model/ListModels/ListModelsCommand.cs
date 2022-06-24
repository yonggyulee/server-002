using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.ListModels;

public class ListModelsCommand : IRequest<ListModelsResponse>
{
    public ListModelsRequest Request { get; set; }

    public ListModelsCommand(ListModelsRequest request)
    {
        Request = request;
    }
}