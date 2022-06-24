using MediatR;
using Mirero.DAQ.Domain.Update.Protos.V1;

namespace Mirero.DAQ.Application.Update.Handlers.Mpp.ListMppVersions;

public sealed class ListMppSetupVersionsCommand : IRequest<ListMppSetupVersionsResponse>
{
    public ListMppSetupVersionsRequest Request { get; set; }

    public ListMppSetupVersionsCommand(ListMppSetupVersionsRequest request)
    {
        Request = request;
    }
}
