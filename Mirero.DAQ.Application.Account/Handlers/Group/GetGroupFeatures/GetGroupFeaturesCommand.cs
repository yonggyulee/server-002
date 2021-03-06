using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.GetGroupFeatures
{
    public class GetGroupFeaturesCommand : IRequest<GetGroupFeaturesResponse>
    {
        public GetGroupFeaturesRequest Request { get; set; }

        public GetGroupFeaturesCommand(GetGroupFeaturesRequest request)
        {
            Request = request;
        }
    }
}
