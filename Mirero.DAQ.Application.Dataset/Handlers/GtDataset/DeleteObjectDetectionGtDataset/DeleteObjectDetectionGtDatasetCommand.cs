using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteObjectDetectionGtDataset;

public class DeleteObjectDetectionGtDatasetCommand : IRequest<ObjectDetectionGtDataset>
{
    public DeleteObjectDetectionGtDatasetRequest Request { get; set; }

    public DeleteObjectDetectionGtDatasetCommand(DeleteObjectDetectionGtDatasetRequest request)
    {
        Request = request;
    }
}