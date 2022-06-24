using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateSegmentationGtDataset;

public class UpdateSegmentationGtDatasetCommand : IRequest<SegmentationGtDataset>
{
    public UpdateSegmentationGtDatasetRequest Request { get; set; }

    public UpdateSegmentationGtDatasetCommand(UpdateSegmentationGtDatasetRequest request)
    {
        Request = request;
    }
}