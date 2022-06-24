using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.DeleteSample;

public class DeleteSampleCommand : IRequest<Sample>
{
    public DeleteSampleRequest Request { get; set; }

    public DeleteSampleCommand(DeleteSampleRequest request)
    {
        Request = request;
    }
}