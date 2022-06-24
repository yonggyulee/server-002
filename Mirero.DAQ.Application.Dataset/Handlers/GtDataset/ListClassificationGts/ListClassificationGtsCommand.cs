using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListClassificationGts;

public class ListClassificationGtsCommand : IRequest<ListClassificationGtsResponse>
{
    public ListClassificationGtsRequest Request { get; set; }

    public ListClassificationGtsCommand(ListClassificationGtsRequest request)
    {
        Request = request;
    }
}