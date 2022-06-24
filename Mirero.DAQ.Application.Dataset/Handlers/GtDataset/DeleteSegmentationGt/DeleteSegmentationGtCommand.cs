using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteSegmentationGt;

public class DeleteSegmentationGtCommand : IRequest<SegmentationGt>
{
    public DeleteSegmentationGtRequest Request { get; set; }

    public DeleteSegmentationGtCommand(DeleteSegmentationGtRequest request)
    {
        Request = request;
    }
}