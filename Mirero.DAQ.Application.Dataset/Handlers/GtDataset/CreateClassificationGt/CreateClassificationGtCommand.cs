using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateClassificationGt;

public class CreateClassificationGtCommand : IRequest<ClassificationGt>
{
    public CreateClassificationGtRequest Request { get; set; }

    public CreateClassificationGtCommand(CreateClassificationGtRequest request)
    {
        Request = request;
    }
}