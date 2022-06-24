using System.IdentityModel.Tokens.Jwt;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateClassificationGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateClassificationGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateObjectDetectionGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateObjectDetectionGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateSegmentationGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateSegmentationGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteClassificationGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteClassificationGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteObjectDetectionGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteObjectDetectionGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteSegmentationGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteSegmentationGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetClassificationGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetClassificationGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetObjectDetectionGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetObjectDetectionGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetSegmentationGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetSegmentationGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListClassificationGtDatasets;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListClassificationGts;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListGtDatasets;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListObjectDetectionGtDatasets;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListObjectDetectionGts;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListSegmentationGtDatasets;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListSegmentationGts;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateClassificationGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateClassificationGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateObjectDetectionGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateObjectDetectionGtDataset;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateSegmentationGt;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateSegmentationGtDataset;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Newtonsoft.Json.Linq;

namespace Mirero.DAQ.Service.Services.Dataset;

public class GrpcGtDatasetService : GtDatasetService.GtDatasetServiceBase
{
    private readonly ILogger<GrpcGtDatasetService> _logger;
    private readonly IMediator _mediator;

    public GrpcGtDatasetService(ILogger<GrpcGtDatasetService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region GtDataset
    public override async Task<ListGtDatasetsResponse> ListGtDatasets(ListGtDatasetsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListGtDatasetsCommand(request), CancellationToken.None);
    }
    #endregion

    #region ClassificationGtDataset
    public override async Task<ClassificationGtDataset> CreateClassificationGtDataset(CreateClassificationGtDatasetRequest request, ServerCallContext context)
    {
        var token = context.RequestHeaders.FirstOrDefault(h => h.Key == "authorization")?.Value.Remove(0, 7);
        var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        var userName = jwtToken.Claims.SingleOrDefault(c => c.Type == "name")?.Value;

        return await _mediator.Send(new CreateClassificationGtDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<ListClassificationGtDatasetsResponse> ListClassificationGtDatasets(ListClassificationGtDatasetsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListClassificationGtDatasetsCommand(request), CancellationToken.None);
    }

    public override async Task<ClassificationGtDataset> GetClassificationGtDataset(GetClassificationGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetClassificationGtDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<ClassificationGtDataset> UpdateClassificationGtDataset(UpdateClassificationGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateClassificationGtDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<ClassificationGtDataset> DeleteClassificationGtDataset(DeleteClassificationGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteClassificationGtDatasetCommand(request), CancellationToken.None);
    }

    #endregion

    #region ObjectDetectionGtDataset
    public override async Task<ObjectDetectionGtDataset> CreateObjectDetectionGtDataset(CreateObjectDetectionGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateObjectDetectionGtDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<ListObjectDetectionGtDatasetsResponse> ListObjectDetectionGtDatasets(ListObjectDetectionGtDatasetsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListObjectDetectionGtDatasetsCommand(request), CancellationToken.None);
    }

    public override async Task<ObjectDetectionGtDataset> GetObjectDetectionGtDataset(GetObjectDetectionGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetObjectDetectionGtDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<ObjectDetectionGtDataset> UpdateObjectDetectionGtDataset(UpdateObjectDetectionGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateObjectDetectionGtDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<ObjectDetectionGtDataset> DeleteObjectDetectionGtDataset(DeleteObjectDetectionGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteObjectDetectionGtDatasetCommand(request), CancellationToken.None);
    }
    #endregion

    #region SegmentationGtDataset
    public override async Task<SegmentationGtDataset> CreateSegmentationGtDataset(CreateSegmentationGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateSegmentationGtDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<ListSegmentationGtDatasetsResponse> ListSegmentationGtDatasets(ListSegmentationGtDatasetsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListSegmentationGtDatasetsCommand(request), CancellationToken.None);
    }

    public override async Task<SegmentationGtDataset> GetSegmentationGtDataset(GetSegmentationGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetSegmentationGtDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<SegmentationGtDataset> UpdateSegmentationGtDataset(UpdateSegmentationGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateSegmentationGtDatasetCommand(request), CancellationToken.None);
    }

    public override async Task<SegmentationGtDataset> DeleteSegmentationGtDataset(DeleteSegmentationGtDatasetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteSegmentationGtDatasetCommand(request), CancellationToken.None);
    }
    #endregion

    #region ClassificationGt
    public override async Task<ClassificationGt> CreateClassificationGt(CreateClassificationGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateClassificationGtCommand(request), CancellationToken.None);
    }

    public override async Task<ListClassificationGtsResponse> ListClassificationGts(ListClassificationGtsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListClassificationGtsCommand(request), CancellationToken.None);
    }

    public override async Task<ClassificationGt> GetClassificationGt(GetClassificationGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetClassificationGtCommand(request), CancellationToken.None);
    }

    public override async Task<ClassificationGt> UpdateClassificationGt(UpdateClassificationGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateClassificationGtCommand(request), CancellationToken.None);
    }

    public override async Task<ClassificationGt> DeleteClassificationGt(DeleteClassificationGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteClassificationGtCommand(request), CancellationToken.None);
    }
    #endregion

    #region ObjectDetectionGt
    public override async Task<ObjectDetectionGt> CreateObjectDetectionGt(CreateObjectDetectionGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateObjectDetectionGtCommand(request), CancellationToken.None);
    }

    public override async Task<ListObjectDetectionGtsResponse> ListObjectDetectionGts(ListObjectDetectionGtsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListObjectDetectionGtsCommand(request), CancellationToken.None);
    }

    public override async Task<ObjectDetectionGt> GetObjectDetectionGt(GetObjectDetectionGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetObjectDetectionGtCommand(request), CancellationToken.None);
    }

    public override async Task<ObjectDetectionGt> UpdateObjectDetectionGt(UpdateObjectDetectionGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateObjectDetectionGtCommand(request), CancellationToken.None);
    }

    public override async Task<ObjectDetectionGt> DeleteObjectDetectionGt(DeleteObjectDetectionGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteObjectDetectionGtCommand(request), CancellationToken.None);
    }
    #endregion

    #region SegmentationGt
    public override async Task<SegmentationGt> CreateSegmentationGt(CreateSegmentationGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateSegmentationGtCommand(request), CancellationToken.None);
    }

    public override async Task<ListSegmentationGtsResponse> ListSegmentationGts(ListSegmentationGtsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListSegmentationGtsCommand(request), CancellationToken.None);
    }

    public override async Task<SegmentationGt> GetSegmentationGt(GetSegmentationGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetSegmentationGtCommand(request), CancellationToken.None);
    }

    public override async Task<SegmentationGt> UpdateSegmentationGt(UpdateSegmentationGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateSegmentationGtCommand(request), CancellationToken.None);
    }

    public override async Task<SegmentationGt> DeleteSegmentationGt(DeleteSegmentationGtRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteSegmentationGtCommand(request), CancellationToken.None);
    }
    #endregion
}