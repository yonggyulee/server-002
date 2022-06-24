using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListObjectDetectionGtDatasets;

public class ListObjectDetectionGtDatasetsCommand : IRequest<ListObjectDetectionGtDatasetsResponse>
{
    public ListObjectDetectionGtDatasetsRequest Request { get; set; }

    public ListObjectDetectionGtDatasetsCommand(ListObjectDetectionGtDatasetsRequest request)
    {
        Request = request;
    }
}