using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListClassificationDataStream;
using Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListObjectDetectionDataStream;
using Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListSamplesStream;
using Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListSegmentationDataStream;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Services.Dataset;

public class GrpcTrainingDataService : TrainingDataService.TrainingDataServiceBase
{
    private readonly ILogger<GrpcTrainingDataService> _logger;
    private readonly IMediator _mediator;

    public GrpcTrainingDataService(ILogger<GrpcTrainingDataService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region TrainingData
    public override async Task ListSamplesStream(ListSamplesStreamRequest request,
        IServerStreamWriter<ListSamplesStreamResponse> responseStream, ServerCallContext context)
    {
        await _mediator.Send(new ListSamplesStreamCommand(request, responseStream), CancellationToken.None);
    }

    public override async Task ListClassificationDataStream(ListClassificationDataStreamRequest request,
        IServerStreamWriter<ListClassificationDataStreamResponse> responseStream,
        ServerCallContext context)
    {
        await _mediator.Send(new ListClassificationDataStreamCommand(request, responseStream), CancellationToken.None);
    }

    public override async Task ListObjectDetectionDataStream(ListObjectDetectionDataStreamRequest request,
        IServerStreamWriter<ListObjectDetectionDataStreamResponse> responseStream,
        ServerCallContext context)
    {
        await _mediator.Send(new ListObjectDetectionDataStreamCommand(request, responseStream), CancellationToken.None);
    }

    public override async Task ListSegmentationDataStream(ListSegmentationDataStreamRequest request,
        IServerStreamWriter<ListSegmentationDataStreamResponse> responseStream,
        ServerCallContext context)
    {
        await _mediator.Send(new ListSegmentationDataStreamCommand(request, responseStream), CancellationToken.None);
    }
    #endregion
}