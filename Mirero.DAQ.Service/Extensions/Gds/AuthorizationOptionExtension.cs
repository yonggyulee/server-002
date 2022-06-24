using Microsoft.AspNetCore.Authorization;
using Mirero.DAQ.Domain.Account.Constants;

namespace Mirero.DAQ.Service.Extensions.Gds;


public static class AuthorizationOptionExtension
{
    public static AuthorizationOptions AddGdsAuthorizationPolicy(this AuthorizationOptions options)
    {
        return options;
    }
}