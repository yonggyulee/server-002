using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetSegmentationGtDataset;

public class GetSegmentationGtDatasetCommand : IRequest<SegmentationGtDataset>
{
    public GetSegmentationGtDatasetRequest Request { get; set; }

    public GetSegmentationGtDatasetCommand(GetSegmentationGtDatasetRequest request)
    {
        Request = request;
    }
}