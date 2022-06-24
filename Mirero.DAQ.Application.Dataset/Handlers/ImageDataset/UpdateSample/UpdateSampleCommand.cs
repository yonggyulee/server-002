using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.UpdateSample;

public class UpdateSampleCommand : IRequest<Sample>
{
    public UpdateSampleRequest Request { get; set; }

    public UpdateSampleCommand(UpdateSampleRequest request)
    {
        Request = request;
    }
}