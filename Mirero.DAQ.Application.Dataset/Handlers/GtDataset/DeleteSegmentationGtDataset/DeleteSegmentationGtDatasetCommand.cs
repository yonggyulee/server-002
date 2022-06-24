using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteSegmentationGtDataset;

public class DeleteSegmentationGtDatasetCommand : IRequest<SegmentationGtDataset>
{
    public DeleteSegmentationGtDatasetRequest Request { get; set; }

    public DeleteSegmentationGtDatasetCommand(DeleteSegmentationGtDatasetRequest request)
    {
        Request = request;
    }
}