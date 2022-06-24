using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateSegmentationGt;

public class UpdateSegmentationGtCommand : IRequest<SegmentationGt>
{
    public UpdateSegmentationGtRequest Request { get; set; }

    public UpdateSegmentationGtCommand(UpdateSegmentationGtRequest request)
    {
        Request = request;
    }
}