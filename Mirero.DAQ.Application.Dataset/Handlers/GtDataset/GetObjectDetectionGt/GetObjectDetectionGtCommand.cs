using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetObjectDetectionGt;

public class GetObjectDetectionGtCommand : IRequest<ObjectDetectionGt>
{
    public GetObjectDetectionGtRequest Request { get; set; }

    public GetObjectDetectionGtCommand(GetObjectDetectionGtRequest request)
    {
        Request = request;
    }
}