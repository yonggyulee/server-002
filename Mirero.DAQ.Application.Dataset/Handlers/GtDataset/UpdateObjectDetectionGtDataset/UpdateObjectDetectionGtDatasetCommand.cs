using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateObjectDetectionGtDataset;

public class UpdateObjectDetectionGtDatasetCommand : IRequest<ObjectDetectionGtDataset>
{
    public UpdateObjectDetectionGtDatasetRequest Request { get; set; }

    public UpdateObjectDetectionGtDatasetCommand(UpdateObjectDetectionGtDatasetRequest request)
    {
        Request = request;
    }
}