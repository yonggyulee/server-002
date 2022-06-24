using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Mirero.DAQ.Service.Middlewares.Authorization;

public class MyAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new ();
    
    public async Task HandleAsync(
        RequestDelegate requestDelegate,
        HttpContext httpContext,
        AuthorizationPolicy authorizationPolicy,
        PolicyAuthorizationResult policyAuthorizationResult)
    {
        if (!policyAuthorizationResult.Succeeded)
        {
            httpContext.Response.Headers.Add("daq-grpc-authorization-errors",
                $"{authorizationPolicy.Requirements} 권한이 없습니다.");
            return;
        }

        await _defaultHandler.HandleAsync(requestDelegate, httpContext, authorizationPolicy, policyAuthorizationResult);
    }
}