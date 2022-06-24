using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.UpdateGroupSystems
{
    public class UpdateGroupSystemsCommand : IRequest<UpdateGroupSystemsResponse>
    {
        public UpdateGroupSystemsRequest Request { get; set; }

        public UpdateGroupSystemsCommand(UpdateGroupSystemsRequest request)
        {
            Request = request;
        }
    }
}
