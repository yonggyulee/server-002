using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetSegmentationGt;

public class GetSegmentationGtCommand : IRequest<SegmentationGt>
{
    public GetSegmentationGtRequest Request { get; set; }

    public GetSegmentationGtCommand(GetSegmentationGtRequest request)
    {
        Request = request;
    }
}