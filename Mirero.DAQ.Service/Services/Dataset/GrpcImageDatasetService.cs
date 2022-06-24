using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.CreateImageDataset;
using Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.CreateSample;
using Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.DeleteImageDataset;
using Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.DeleteSample;
using Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.GetImageDataset;
using Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.GetSample;
using Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.ListImageDatasets;
using Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.ListSamples;
using Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.UpdateImageDataset;
using Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.UpdateSample;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Services.Dataset;

public class GrpcImageDatasetService : ImageDatasetService.ImageDatasetServiceBase
{
    private readonly ILogger<GrpcImageDatasetService> _logger;
    private readonly IMediator _mediator;

    public GrpcImageDatasetService(ILogger<GrpcImageDatasetService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region ImageDataset
    public override async Task<ImageDataset> CreateImageDataset(CreateImageDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateImageDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<ListImageDatasetsResponse> ListImageDatasets(ListImageDatasetsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListImageDatasetsCommand(request), CancellationToken.None);
    }

    public override async Task<ImageDataset> GetImageDataset(GetImageDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetImageDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<ImageDataset> UpdateImageDataset(UpdateImageDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateImageDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<ImageDataset> DeleteImageDataset(DeleteImageDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteImageDatasetCommand(request), CancellationToken.None);
    }
    #endregion

    #region Sample
    public override async Task<Sample> CreateSample(CreateSampleRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateSampleCommand(request), CancellationToken.None);
    }

    public override async Task<ListSamplesResponse> ListSamples(ListSamplesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListSamplesCommand(request), CancellationToken.None);
    }

    public override async Task<Sample> GetSample(GetSampleRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetSampleCommand(request), CancellationToken.None);
    }

    public override async Task<Sample> UpdateSample(UpdateSampleRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateSampleCommand(request), CancellationToken.None);
    }

    public override async Task<Sample> DeleteSample(DeleteSampleRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteSampleCommand(request), CancellationToken.None);
    }
    #endregion
}