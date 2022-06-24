using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.CreateSample;

public class CreateSampleCommand : IRequest<Sample>
{
    public CreateSampleRequest Request { get; set; }

    public CreateSampleCommand(CreateSampleRequest request)
    {
        Request = request;
    }
}