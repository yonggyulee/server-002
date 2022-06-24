using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteObjectDetectionGt;

public class DeleteObjectDetectionGtCommand : IRequest<ObjectDetectionGt>
{
    public DeleteObjectDetectionGtRequest Request { get; set; }

    public DeleteObjectDetectionGtCommand(DeleteObjectDetectionGtRequest request)
    {
        Request = request;
    }
}