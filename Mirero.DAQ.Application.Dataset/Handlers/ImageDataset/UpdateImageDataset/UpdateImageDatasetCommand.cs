using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.UpdateImageDataset;

public class UpdateImageDatasetCommand : IRequest<Domain.Dataset.Protos.V1.ImageDataset>
{
    public UpdateImageDatasetRequest Request { get; set; }

    public UpdateImageDatasetCommand(UpdateImageDatasetRequest request)
    {
        Request = request;
    }
}