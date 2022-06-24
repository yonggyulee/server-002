using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteClassificationGtDataset;

public class DeleteClassificationGtDatasetCommand : IRequest<ClassificationGtDataset>
{
    public DeleteClassificationGtDatasetRequest Request { get; set; }

    public DeleteClassificationGtDatasetCommand(DeleteClassificationGtDatasetRequest request)
    {
        Request = request;
    }
}