using Microsoft.AspNetCore.Authorization;
using Mirero.DAQ.Domain.Account.Constants;

namespace Mirero.DAQ.Service.Extensions.Inference;

public static class AuthorizationOptionExtension
{
    public static AuthorizationOptions AddInferenceAuthorizationPolicy(this AuthorizationOptions options)
    {
        return options;
    }
}
