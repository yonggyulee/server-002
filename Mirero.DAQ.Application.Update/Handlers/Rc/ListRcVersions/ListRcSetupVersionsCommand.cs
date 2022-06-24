using MediatR;
using Mirero.DAQ.Domain.Update.Protos.V1;

namespace Mirero.DAQ.Application.Update.Handlers.Rc.ListRcVersions;

public sealed class ListRcSetupVersionsCommand : IRequest<ListRcSetupVersionsResponse>
{
    public ListRcSetupVersionsRequest Request { get; set; }

    public ListRcSetupVersionsCommand(ListRcSetupVersionsRequest request)
    {
        Request = request;
    }
}
