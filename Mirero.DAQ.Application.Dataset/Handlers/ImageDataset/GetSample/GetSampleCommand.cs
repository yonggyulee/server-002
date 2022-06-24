using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.GetSample;

public class GetSampleCommand : IRequest<Sample>
{
    public GetSampleRequest Request { get; set; }

    public GetSampleCommand(GetSampleRequest request)
    {
        Request = request;
    }
}