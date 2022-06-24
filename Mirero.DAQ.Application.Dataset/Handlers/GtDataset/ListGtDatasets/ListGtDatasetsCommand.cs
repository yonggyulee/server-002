using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListGtDatasets;

public class ListGtDatasetsCommand : IRequest<ListGtDatasetsResponse>
{
    public ListGtDatasetsRequest Request { get; set; }

    public ListGtDatasetsCommand(ListGtDatasetsRequest request)
    {
        Request = request;
    }
}