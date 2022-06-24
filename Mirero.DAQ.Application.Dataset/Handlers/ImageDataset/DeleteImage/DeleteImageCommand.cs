using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.DeleteImage;

public class DeleteImageCommand : IRequest<Image>
{
    public DeleteImageRequest Request { get; set; }

    public DeleteImageCommand(DeleteImageRequest request)
    {
        Request = request;
    }
}