using Microsoft.AspNetCore.Authorization;
using Mirero.DAQ.Domain.Account.Constants;

namespace Mirero.DAQ.Service.Extensions.Workflow;

public static class AuthorizationOptionExtension
{
    public static AuthorizationOptions AddWorkflowAuthorizationPolicy(this AuthorizationOptions options)
    {
        return options;
    }
}