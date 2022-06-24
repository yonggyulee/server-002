using Grpc.Core;
using Grpc.Core.Interceptors;
using Mirero.DAQ.Service.Interceptors.Validations;

namespace Mirero.DAQ.Service.Interceptors;

public class ValidationInterceptor : Interceptor
{
    private readonly IValidatorLocator _locator;
    private readonly IValidatorErrorMessageHandler _handler;

    public ValidationInterceptor(IValidatorLocator locator, IValidatorErrorMessageHandler handler)
    {
        _locator = locator ?? throw new ArgumentNullException(nameof(locator));
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
    }

    private async Task _ValidateRequest<TRequest>(TRequest request) where TRequest : class
    {
        if (_locator.TryGetValidator<TRequest>(out var validator))
        {
            var results = await validator.ValidateAsync(request);

            if (!results.IsValid && results.Errors.Any())
            {
                var message = await _handler.HandleAsync(results.Errors);
                var validationMetadata = results.Errors.ToValidationMetadata();
                throw new RpcException(new Status(StatusCode.InvalidArgument, message), validationMetadata);
            }
        }
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        await _ValidateRequest(request);
        return await continuation(request, context);
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        await _ValidateRequest(request);
        await continuation(request, responseStream, context);
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        var validatingRequestStream = new ValidatingAsyncStreamReader<TRequest>(requestStream, _ValidateRequest);
        return await continuation(validatingRequestStream, context);
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        var validatingRequestStream = new ValidatingAsyncStreamReader<TRequest>(requestStream, _ValidateRequest);
        await continuation(validatingRequestStream, responseStream, context);
    }
}