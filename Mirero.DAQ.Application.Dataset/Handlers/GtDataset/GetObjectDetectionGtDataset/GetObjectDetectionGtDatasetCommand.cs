using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetObjectDetectionGtDataset;

public class GetObjectDetectionGtDatasetCommand : IRequest<ObjectDetectionGtDataset>
{
    public GetObjectDetectionGtDatasetRequest Request { get; set; }

    public GetObjectDetectionGtDatasetCommand(GetObjectDetectionGtDatasetRequest request)
    {
        Request = request;
    }
}