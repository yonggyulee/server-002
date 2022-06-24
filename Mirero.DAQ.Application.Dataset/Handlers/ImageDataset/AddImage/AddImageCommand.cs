using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.AddImage;

public class AddImageCommand : IRequest<Image>
{
    public Image Request { get; set; }

    public AddImageCommand(Image request)
    {
        Request = request;
    }
}