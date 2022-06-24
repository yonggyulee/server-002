using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.CreateImageDataset;

public class CreateImageDatasetCommand : IRequest<Domain.Dataset.Protos.V1.ImageDataset>
{
    public CreateImageDatasetRequest Request { get; set; }

    public CreateImageDatasetCommand(CreateImageDatasetRequest request)
    {
        Request = request;
    }
}