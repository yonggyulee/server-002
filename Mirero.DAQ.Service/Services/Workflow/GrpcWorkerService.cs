using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Workflow.Handlers.Worker.CreateWorker;
using Mirero.DAQ.Application.Workflow.Handlers.Worker.DeleteWorker;
using Mirero.DAQ.Application.Workflow.Handlers.Worker.ListWorkers;
using Mirero.DAQ.Application.Workflow.Handlers.Worker.StartWorker;
using Mirero.DAQ.Application.Workflow.Handlers.Worker.StopWorker;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Service.Services.Workflow
{
    public class GrpcWorkerService : WorkerService.WorkerServiceBase
    {
        private readonly ILogger<GrpcWorkerService> _logger;
        private readonly IMediator _mediator;

        public GrpcWorkerService(ILogger<GrpcWorkerService> logger
            , IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override async Task<ListWorkersResponse> ListWorkers(ListWorkersRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new ListWorkersCommand(request), context.CancellationToken);
        }

        public override async Task<Worker> CreateWorker(CreateWorkerRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new CreateWorkerCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> StartWorker(StartWorkerRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new StartWorkerCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> StopWorker(StopWorkerRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new StopWorkerCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> DeleteWorker(DeleteWorkerRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new DeleteWorkerCommand(request), context.CancellationToken);
        }
    }
}
