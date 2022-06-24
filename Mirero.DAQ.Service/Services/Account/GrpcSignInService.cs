using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Account.Handlers.SignIn.RefreshAccessToken;
using Mirero.DAQ.Application.Account.Handlers.SignIn.SignIn;
using Mirero.DAQ.Application.Account.Handlers.SignIn.SignOut;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Serilog;

using SignInService = Mirero.DAQ.Domain.Account.Protos.V1.SignInService;

namespace Mirero.DAQ.Service.Services.Account;

public class GrpcSignInService : SignInService.SignInServiceBase
{
    private readonly ILogger<GrpcSignInService> _logger;
    private readonly IMediator _mediator;


    public GrpcSignInService(ILogger<GrpcSignInService> logger, IMediator mediator)
    {
        _logger  = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new SignInCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> SignOut(SignOutRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new SignOutCommand(request), CancellationToken.None);
    }

    public override async Task<RefreshAccessTokenResponse> RefreshAccessToken(RefreshAccessTokenRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new RefreshAccessTokenCommand(request), CancellationToken.None);
    }
}