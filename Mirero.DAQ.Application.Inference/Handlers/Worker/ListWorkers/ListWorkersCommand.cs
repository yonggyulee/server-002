using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Worker.ListWorkers;

public class ListWorkersCommand : IRequest<ListWorkersResponse>
{
    public ListWorkersRequest Request { get; set; }

    public ListWorkersCommand(ListWorkersRequest request)
    {
        Request = request;
    }
}