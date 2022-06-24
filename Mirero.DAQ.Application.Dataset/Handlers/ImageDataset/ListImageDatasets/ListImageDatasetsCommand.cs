using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.ListImageDatasets;

public class ListImageDatasetsCommand : IRequest<ListImageDatasetsResponse>
{
    public ListImageDatasetsRequest Request { get; set; }

    public ListImageDatasetsCommand(ListImageDatasetsRequest request)
    {
        Request = request;
    }
}