using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetClassificationGtDataset;

public class GetClassificationGtDatasetCommand : IRequest<ClassificationGtDataset>
{
    public GetClassificationGtDatasetRequest Request { get; set; }

    public GetClassificationGtDatasetCommand(GetClassificationGtDatasetRequest request)
    {
        Request = request;
    }
}