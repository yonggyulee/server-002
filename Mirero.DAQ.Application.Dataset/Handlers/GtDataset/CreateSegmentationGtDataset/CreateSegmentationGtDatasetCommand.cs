using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateSegmentationGtDataset;

public class CreateSegmentationGtDatasetCommand : IRequest<SegmentationGtDataset>
{
    public CreateSegmentationGtDatasetRequest Request { get; set; }

    public CreateSegmentationGtDatasetCommand(CreateSegmentationGtDatasetRequest request)
    {
        Request = request;
    }
}