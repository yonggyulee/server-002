using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.Handlers.Server.DeleteServer;
using Mirero.DAQ.Application.Inference.Handlers.Server.ListServers;
using Mirero.DAQ.Application.Inference.Handlers.Server.UpdateServer;
using Mirero.DAQ.Application.Inference.Handlers.Server.CreateServer;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Service.Services.Inference;

public class GrpcServerService : ServerService.ServerServiceBase
{
    private readonly ILogger<GrpcInferenceService> _logger;
    private readonly IMediator _mediator;

    public GrpcServerService(ILogger<GrpcInferenceService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task<ListServersResponse> ListServers(ListServersRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListServersCommand(request), CancellationToken.None);
    }

    public override async Task<Server> CreateServer(CreateServerRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateServerCommand(request), CancellationToken.None);
    }

    public override async Task<Server> UpdateServer(UpdateServerRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateServerCommand(request), CancellationToken.None);
    }

    public override async Task<Server> DeleteServer(DeleteServerRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteServerCommand(request), CancellationToken.None);
    }
}