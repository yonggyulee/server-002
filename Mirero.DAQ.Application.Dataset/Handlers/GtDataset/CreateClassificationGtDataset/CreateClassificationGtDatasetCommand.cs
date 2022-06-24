using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateClassificationGtDataset;

public class CreateClassificationGtDatasetCommand : IRequest<ClassificationGtDataset>
{
    public CreateClassificationGtDatasetRequest Request { get; set; }

    public CreateClassificationGtDatasetCommand(CreateClassificationGtDatasetRequest request)
    {
        Request = request;
    }
}