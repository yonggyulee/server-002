using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Account.Constants;

namespace Mirero.DAQ.Service.Extensions.Account;

public static class AuthorizationOptionExtension
{
    public static AuthorizationOptions AddAccountAuthorizationPolicy(this AuthorizationOptions options)
    {
        options.AddPolicy(AccountPolicy.DeleteUser, policy => { policy.RequireClaim(PrivilegeId.DeleteUser); });
        options.AddPolicy(AccountPolicy.CreateGroup, policy => { policy.RequireClaim(PrivilegeId.CreateGroup); });
        options.AddPolicy(AccountPolicy.DeleteGroup, policy => { policy.RequireClaim(PrivilegeId.DeleteGroup); });

        return options;
    }
}