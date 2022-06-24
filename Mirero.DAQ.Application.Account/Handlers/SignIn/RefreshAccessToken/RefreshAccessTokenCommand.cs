using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.SignIn.RefreshAccessToken
{
    public class RefreshAccessTokenCommand : IRequest<RefreshAccessTokenResponse>
    {
        public RefreshAccessTokenRequest Request { get; set; }

        public RefreshAccessTokenCommand(RefreshAccessTokenRequest request)
        {
            Request = request;
        }
    }
}
