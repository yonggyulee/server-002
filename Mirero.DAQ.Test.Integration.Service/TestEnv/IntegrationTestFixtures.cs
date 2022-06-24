using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service;

public class CurrentTestUser
{
    public static readonly CurrentTestUser InvalidUser = new CurrentTestUser
    {
        Id = string.Empty,
        AccessToken = string.Empty,
        RefreshToken = string.Empty
    };

    public string Id { get; set; }

    private string? _name;
    public string? Name => (_name ??= 
        (new JwtSecurityTokenHandler().ReadToken(AccessToken) as JwtSecurityToken)?.Claims
        .SingleOrDefault(c => c.Type == "name")?.Value);
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

public class ClientAuthInterceptor : Interceptor
{
    private readonly ApiServiceFixture _fixture;

    public ClientAuthInterceptor(ApiServiceFixture fixture)
    {
        _fixture = fixture;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var newContext = WithAuthHeader(context);
        return base.AsyncUnaryCall(request, newContext, continuation);
    }

    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        return base.AsyncClientStreamingCall(WithAuthHeader(context), continuation);
    }

    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        return base.AsyncServerStreamingCall(request, WithAuthHeader(context), continuation);
    }

    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        return base.AsyncDuplexStreamingCall(WithAuthHeader(context), continuation);
    }


    private ClientInterceptorContext<TRequest, TResponse> WithAuthHeader<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context) where TRequest : class where TResponse : class
    {
        var headers = new Metadata()
        {
            { "Authorization", $"Bearer {_fixture.CurrentTestUser.AccessToken}" }
        };
        var newOptions = context.Options.WithHeaders(headers);
        var newContext = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method,
                context.Host,
                newOptions)
            ;
        return newContext;
    }
}

public class TestSignInContext : IDisposable
{
    private readonly ApiServiceFixture _fixture;

    public TestSignInContext(ApiServiceFixture fixture)
    {
        _fixture = fixture;
    }

    public bool SignedIn { get; set; }

    public async Task<bool> SignInAsSuperAdministrator()
    {
        SignedIn = await _fixture.SignInAsync("ID_ksy1", "Mirero2816!");
        return SignedIn;
    }

    public async Task<bool> SingInAsGroupAdministrator()
    {
        SignedIn = await _fixture.SignInAsync("ID_ksy2", "Mirero2816!");
        return SignedIn;
    }

    public async Task<bool> SignInAsMaintainer()
    {
        SignedIn = await _fixture.SignInAsync("ID_ksy3", "Mirero2816!");
        return SignedIn;
    }

    public async Task<bool> SignInAsDeveloper()
    {
        SignedIn = await _fixture.SignInAsync("ID_ksy4", "Mirero2816!");
        return SignedIn;
    }

    public void Dispose()
    {
        _fixture.SignOut();
    }
}

[CollectionDefinition("IntegrationTest")]
public class IntegrationTestFixtures : ICollectionFixture<ApiServiceFixture>
{
}