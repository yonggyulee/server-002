using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.DeleteImageDataset;

public class DeleteImageDatasetCommand : IRequest<Domain.Dataset.Protos.V1.ImageDataset>
{
    public DeleteImageDatasetRequest Request { get; set; }

    public DeleteImageDatasetCommand(DeleteImageDatasetRequest request)
    {
        Request = request;
    }
}