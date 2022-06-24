using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.GetImageDataset;

public class GetImageDatasetCommand : IRequest<Domain.Dataset.Protos.V1.ImageDataset>
{
    public GetImageDatasetRequest Request { get; set; }

    public GetImageDatasetCommand(GetImageDatasetRequest request)
    {
        Request = request;
    }
}