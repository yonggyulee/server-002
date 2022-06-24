using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using Mirero.DAQ.Infrastructure.Identity;

namespace Mirero.DAQ.Application.Account.Handlers.SignIn
{
    public class SignInHandler
    {
        protected SignInHandler(ILogger<SignInHandler> logger, IMapper mapper, IConfiguration configuration,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory, RequesterContext requesterContext,
            ITokenManager tokenManager)
        {

        }
    }
}
