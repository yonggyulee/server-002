using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListSegmentationGtDatasets;

public class ListSegmentationGtDatasetsCommand : IRequest<ListSegmentationGtDatasetsResponse>
{
    public ListSegmentationGtDatasetsRequest Request { get; set; }

    public ListSegmentationGtDatasetsCommand(ListSegmentationGtDatasetsRequest request)
    {
        Request = request;
    }
}