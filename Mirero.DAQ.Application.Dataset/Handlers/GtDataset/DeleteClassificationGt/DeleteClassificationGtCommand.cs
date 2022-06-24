using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteClassificationGt;

public class DeleteClassificationGtCommand : IRequest<ClassificationGt>
{
    public DeleteClassificationGtRequest Request { get; set; }

    public DeleteClassificationGtCommand(DeleteClassificationGtRequest request)
    {
        Request = request;
    }
}