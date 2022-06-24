using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.SignIn.SignIn
{
    public class SignInCommand : IRequest<SignInResponse>
    {
        public SignInRequest Request { get; set; }

        public SignInCommand(SignInRequest request)
        {
            Request = request;
        }
    }
}
