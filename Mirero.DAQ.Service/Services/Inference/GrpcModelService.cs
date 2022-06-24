using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.Handlers.Model.CreateModel;
using Mirero.DAQ.Application.Inference.Handlers.Model.CreateModelVersion;
using Mirero.DAQ.Application.Inference.Handlers.Model.DeleteModel;
using Mirero.DAQ.Application.Inference.Handlers.Model.DeleteModelVersion;
using Mirero.DAQ.Application.Inference.Handlers.Model.DownloadModelVersionStream;
using Mirero.DAQ.Application.Inference.Handlers.Model.GetDefaultModelVersion;
using Mirero.DAQ.Application.Inference.Handlers.Model.ListModels;
using Mirero.DAQ.Application.Inference.Handlers.Model.ListModelVersions;
using Mirero.DAQ.Application.Inference.Handlers.Model.RegisterModelVersion;
using Mirero.DAQ.Application.Inference.Handlers.Model.SetDefaultModelVersion;
using Mirero.DAQ.Application.Inference.Handlers.Model.UnloadModelVersion;
using Mirero.DAQ.Application.Inference.Handlers.Model.UpdateModel;
using Mirero.DAQ.Application.Inference.Handlers.Model.UpdateModelVersion;
using Mirero.DAQ.Application.Inference.Handlers.Model.UploadModelVersionStream;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Service.Services.Inference;

public class GrpcModelService : ModelService.ModelServiceBase
{
    private readonly ILogger<GrpcInferenceService> _logger;
    private readonly IMediator _mediator;

    public GrpcModelService(ILogger<GrpcInferenceService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region Model

    public override async Task<ListModelsResponse> ListModels(ListModelsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListModelsCommand(request), CancellationToken.None);
    }

    public override async Task<Model> CreateModel(CreateModelRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateModelCommand(request), CancellationToken.None);
    }

    public override async Task<Model> UpdateModel(UpdateModelRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateModelCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> DeleteModel(DeleteModelRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteModelCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> SetDefaultModelVersion(SetDefaultModelVersionRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new SetDefaultModelVersionCommand(request), CancellationToken.None);
    }

    public override async Task<ModelVersion> GetDefaultModelVersion(GetDefaultModelVersionRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetDefaultModelVersionCommand(request), CancellationToken.None);
    }

    #endregion

    #region ModelVersion

    public override async Task<ListModelVersionsResponse> ListModelVersions(ListModelVersionsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListModelVersionsCommand(request), CancellationToken.None);
    }

    public override async Task<ModelVersion> CreateModelVersion(CreateModelVersionRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateModelVersionCommand(request), CancellationToken.None);
    }

    public override async Task<ModelVersion> UpdateModelVersion(UpdateModelVersionRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateModelVersionCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> DeleteModelVersion(DeleteModelVersionRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteModelVersionCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> UploadModelVersionStream(IAsyncStreamReader<UploadModelVersionRequest> requestStream, ServerCallContext context)
    {
        return await _mediator.Send(new UploadModelVersionStreamCommand(requestStream), CancellationToken.None);
    }

    public override async Task DownloadModelVersionStream(DownloadModelVersionRequest request, IServerStreamWriter<DownloadModelVersionResponse> responseStream,
        ServerCallContext context)
    {
        await _mediator.Send(new DownloadModelVersionStreamCommand(request, responseStream), CancellationToken.None);
    }

    #endregion
    

    public override async Task<LoadModelResponse> LoadModelVersion(LoadModelRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new LoadModelVersionCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> UnloadModelVersion(UnloadModelVersionRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UnloadModelVersionCommand(request), CancellationToken.None);
    }
}