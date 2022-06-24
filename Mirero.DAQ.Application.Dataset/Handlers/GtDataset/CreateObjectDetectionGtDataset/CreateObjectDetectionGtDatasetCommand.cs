using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateObjectDetectionGtDataset;

public class CreateObjectDetectionGtDatasetCommand : IRequest<ObjectDetectionGtDataset>
{
    public CreateObjectDetectionGtDatasetRequest Request { get; set; }

    public CreateObjectDetectionGtDatasetCommand(CreateObjectDetectionGtDatasetRequest request)
    {
        Request = request;
    }
}