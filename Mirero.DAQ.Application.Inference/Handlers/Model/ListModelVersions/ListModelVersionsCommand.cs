using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.ListModelVersions;

public class ListModelVersionsCommand : IRequest<ListModelVersionsResponse>
{
    public ListModelVersionsRequest Request { get; set; }

    public ListModelVersionsCommand(ListModelVersionsRequest request)
    {
        Request = request;
    }
}