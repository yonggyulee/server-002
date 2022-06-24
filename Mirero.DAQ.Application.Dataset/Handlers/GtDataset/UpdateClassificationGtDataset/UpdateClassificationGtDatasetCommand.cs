using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateClassificationGtDataset;

public class UpdateClassificationGtDatasetCommand : IRequest<ClassificationGtDataset>
{
    public UpdateClassificationGtDatasetRequest Request { get; set; }

    public UpdateClassificationGtDatasetCommand(UpdateClassificationGtDatasetRequest request)
    {
        Request = request;
    }
}