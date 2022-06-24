using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.UpdateGroupFeatures
{
    public class UpdateGroupFeaturesCommand : IRequest<UpdateGroupFeaturesResponse>
    {
        public UpdateGroupFeaturesRequest Request { get; set; }

        public UpdateGroupFeaturesCommand(UpdateGroupFeaturesRequest request)
        {
            Request = request;
        }
    }
}
