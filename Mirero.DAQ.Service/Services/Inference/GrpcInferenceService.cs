using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.Handlers.Inference.Prediction;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Service.Services.Inference;

public class GrpcInferenceService : InferenceService.InferenceServiceBase
{
    private readonly ILogger<GrpcInferenceService> _logger;
    private readonly IMediator _mediator;
    
    public GrpcInferenceService(ILogger<GrpcInferenceService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task<PredictionResponse> Prediction(PredictionRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new PredictionCommand(request), CancellationToken.None);
    }
}