using System.IdentityModel.Tokens.Jwt;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Mirero.DAQ.Domain.Common.Data;

namespace Mirero.DAQ.Service.Interceptors;

public class RequesterContextInterceptor : Interceptor
{
    private readonly RequesterContext _requesterContext;

    public RequesterContextInterceptor(RequesterContext requesterContext)
    {
        _requesterContext = requesterContext ?? throw new ArgumentNullException(nameof(requesterContext));
    }

    private void _ParseJwtToken(ServerCallContext context, RequesterContext requesterContext)
    {
        var token = context.RequestHeaders.FirstOrDefault(h => h.Key == "authorization")?.Value.Remove(0, 7);
        if (token == null) return;

        var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        var userName = jwtToken?.Claims.SingleOrDefault(c => c.Type == "name")?.Value;

        requesterContext.UserName = userName;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _ParseJwtToken(context, _requesterContext);
        return await continuation(request, context);
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        _ParseJwtToken(context, _requesterContext);
        await continuation(request, responseStream, context);
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        _ParseJwtToken(context, _requesterContext);
        return await continuation(requestStream, context);
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        _ParseJwtToken(context, _requesterContext);
        await continuation(requestStream, responseStream, context);
    }
}