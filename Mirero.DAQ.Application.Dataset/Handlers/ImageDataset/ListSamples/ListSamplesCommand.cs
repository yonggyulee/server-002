using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.ListSamples;

public class ListSamplesCommand : IRequest<ListSamplesResponse>
{
    public ListSamplesRequest Request { get; set; }

    public ListSamplesCommand(ListSamplesRequest request)
    {
        Request = request;
    }
}