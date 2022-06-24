using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateSegmentationGt;

public class CreateSegmentationGtCommand : IRequest<SegmentationGt>
{
    public CreateSegmentationGtRequest Request { get; set; }

    public CreateSegmentationGtCommand(CreateSegmentationGtRequest request)
    {
        Request = request;
    }
}