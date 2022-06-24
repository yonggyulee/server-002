using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateObjectDetectionGt;

public class CreateObjectDetectionGtCommand : IRequest<ObjectDetectionGt>
{
    public CreateObjectDetectionGtRequest Request { get; set; }

    public CreateObjectDetectionGtCommand(CreateObjectDetectionGtRequest request)
    {
        Request = request;
    }
}