using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Workflow.Handlers.Server.CreateServer;
using Mirero.DAQ.Application.Workflow.Handlers.Server.DeleteServer;
using Mirero.DAQ.Application.Workflow.Handlers.Server.ListServers;
using Mirero.DAQ.Application.Workflow.Handlers.Server.UpdateServer;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Service.Services.Workflow
{
    public class GrpcServerService : ServerService.ServerServiceBase
    {
        private readonly ILogger<GrpcServerService> _logger;
        private readonly IMediator _mediator;

        public GrpcServerService(ILogger<GrpcServerService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override async Task<ListServersResponse> ListServers(ListServersRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new ListServersCommand(request), context.CancellationToken);
        }

        public override async Task<Server> CreateServer(CreateServerRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new CreateServerCommand(request), context.CancellationToken);
        }

        public override async Task<Server> UpdateServer(UpdateServerRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new UpdateServerCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> DeleteServer(DeleteServerRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new DeleteServerCommand(request), context.CancellationToken);
        }
    }
}
