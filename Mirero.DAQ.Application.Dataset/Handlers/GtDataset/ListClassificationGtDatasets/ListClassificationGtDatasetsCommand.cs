using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListClassificationGtDatasets;

public class ListClassificationGtDatasetsCommand : IRequest<ListClassificationGtDatasetsResponse>
{
    public ListClassificationGtDatasetsRequest Request { get; set; }

    public ListClassificationGtDatasetsCommand(ListClassificationGtDatasetsRequest request)
    {
        Request = request;
    }
}