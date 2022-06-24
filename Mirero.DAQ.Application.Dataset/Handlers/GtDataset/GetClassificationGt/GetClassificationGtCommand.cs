using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetClassificationGt;

public class GetClassificationGtCommand : IRequest<ClassificationGt>
{
    public GetClassificationGtRequest Request { get; set; }

    public GetClassificationGtCommand(GetClassificationGtRequest request)
    {
        Request = request;
    }
}