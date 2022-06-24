using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListSegmentationGts;

public class ListSegmentationGtsCommand : IRequest<ListSegmentationGtsResponse>
{
    public ListSegmentationGtsRequest Request { get; set; }

    public ListSegmentationGtsCommand(ListSegmentationGtsRequest request)
    {
        Request = request;
    }
}