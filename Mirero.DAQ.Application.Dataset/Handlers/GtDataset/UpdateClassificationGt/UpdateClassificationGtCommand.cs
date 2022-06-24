using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateClassificationGt;

public class UpdateClassificationGtCommand : IRequest<ClassificationGt>
{
    public UpdateClassificationGtRequest Request { get; set; }

    public UpdateClassificationGtCommand(UpdateClassificationGtRequest request)
    {
        Request = request;
    }
}