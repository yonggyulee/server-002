using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.Group.ListFeatures
{
    public class ListFeaturesCommand : IRequest<ListFeaturesResponse>
    {
        private ListFeaturesRequest Request { get; set; }

        public ListFeaturesCommand(ListFeaturesRequest request)
        {
            Request = request;
        }
    }
}
