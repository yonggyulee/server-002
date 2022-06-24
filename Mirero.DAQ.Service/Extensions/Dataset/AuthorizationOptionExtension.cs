using Microsoft.AspNetCore.Authorization;
using Mirero.DAQ.Domain.Account.Constants;

namespace Mirero.DAQ.Service.Extensions.Dataset;

public static class AuthorizationOptionExtension
{
    public static AuthorizationOptions AddDatasetAuthorizationPolicy(this AuthorizationOptions options)
    {
        return options;
    }
}