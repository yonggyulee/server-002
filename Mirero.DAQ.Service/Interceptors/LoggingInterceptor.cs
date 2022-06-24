using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Mirero.DAQ.Service.Interceptors;

public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;
    
    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private void _LogMetadata(Metadata headers, string key)
    {
        var headerValue = headers.SingleOrDefault(h => h.Key == key)?.Value;
        _logger.LogTrace($"{key}: {headerValue ?? "(unknown)"}");
    }
    
    private void _LogCall<TRequest, TResponse>(MethodType methodType, ServerCallContext context) 
        where TRequest : class
        where TResponse : class
    {
        _logger.LogInformation($"gRPC Start");
        _LogMetadata(context.RequestHeaders, "caller-user");
        _LogMetadata(context.RequestHeaders, "caller-machine");
        _LogMetadata(context.RequestHeaders, "caller-os");
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, 
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _LogCall<TRequest, TResponse>(MethodType.Unary, context);
        return await continuation(request, context);
        //return base.UnaryServerHandler(request, context, continuation);
    }
    
}