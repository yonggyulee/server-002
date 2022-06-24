using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListObjectDetectionGts;

public class ListObjectDetectionGtsCommand : IRequest<ListObjectDetectionGtsResponse>
{
    public ListObjectDetectionGtsRequest Request { get; set; }

    public ListObjectDetectionGtsCommand(ListObjectDetectionGtsRequest request)
    {
        Request = request;
    }
}