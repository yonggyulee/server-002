using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mirero.DAQ.Domain.Account.Protos.V1;

namespace Mirero.DAQ.Application.Account.Handlers.SignIn.SignOut
{
    public class SignOutCommand : IRequest<Empty>
    {
        public SignOutRequest Request { get; set; }

        public SignOutCommand(SignOutRequest request)
        {
            Request = request;
        }
    }
}
