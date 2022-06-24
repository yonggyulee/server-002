using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.Handlers.Worker.CreateWorker;
using Mirero.DAQ.Application.Inference.Handlers.Worker.ListWorkers;
using Mirero.DAQ.Application.Inference.Handlers.Worker.RemoveWorker;
using Mirero.DAQ.Application.Inference.Handlers.Worker.StartWorker;
using Mirero.DAQ.Application.Inference.Handlers.Worker.StopWorker;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Service.Services.Inference;

public class GrpcWorkerService : WorkerService.WorkerServiceBase
{
    private readonly ILogger<GrpcInferenceService> _logger;
    private readonly IMediator _mediator;

    public GrpcWorkerService(ILogger<GrpcInferenceService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region Worker

    public override async Task<ListWorkersResponse> ListWorkers(ListWorkersRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListWorkersCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> CreateWorker(CreateWorkerRequest request, ServerCallContext context)
    {
        await _mediator.Send(new CreateWorkerCommand(request), CancellationToken.None);
        return new Empty();
    }

    public override async Task<Empty> StartWorker(StartWorkerRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new StartWorkerCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> StopWorker(StopWorkerRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new StopWorkerCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> RemoveWorker(RemoveWorkerRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new RemoveWorkerCommand(request), CancellationToken.None);
    }

    #endregion
}