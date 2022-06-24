using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateObjectDetectionGt;

public class UpdateObjectDetectionGtCommand : IRequest<ObjectDetectionGt>
{
    public UpdateObjectDetectionGtRequest Request { get; set; }

    public UpdateObjectDetectionGtCommand(UpdateObjectDetectionGtRequest request)
    {
        Request = request;
    }
}