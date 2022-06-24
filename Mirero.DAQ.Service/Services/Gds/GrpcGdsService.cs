using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Gds.Handlers.Gds.CreateFloorPlan;
using Mirero.DAQ.Application.Gds.Handlers.Gds.CreateGds;
using Mirero.DAQ.Application.Gds.Handlers.Gds.DeleteGds;
using Mirero.DAQ.Application.Gds.Handlers.Gds.DownloadGdsStream;
using Mirero.DAQ.Application.Gds.Handlers.Gds.ListGdses;
using Mirero.DAQ.Application.Gds.Handlers.Gds.UpdateFloorPlan;
using Mirero.DAQ.Application.Gds.Handlers.Gds.UploadGdsStream;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Services.Gds;

public class GrpcGdsService : GdsService.GdsServiceBase
{
    private readonly ILogger<GrpcGdsService> _logger;
    private readonly IMediator _mediator;
    public GrpcGdsService(ILogger<GrpcGdsService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region Gds
    public override async Task<CreateGdsResponse> CreateGds(CreateGdsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateGdsCommand(request), context.CancellationToken);
    }
    public override async Task<ListGdsesResponse> ListGdses(ListGdsesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListGdsesCommand(request), context.CancellationToken);
    }

    public override async Task<Empty> DeleteGds(DeleteGdsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteGdsCommand(request), context.CancellationToken);
    }

    public override async Task<Empty> UploadGdsStream(IAsyncStreamReader<UploadGdsStreamRequest> requestStream, ServerCallContext context)
    {
        return await _mediator.Send(new UploadGdsStreamCommand(requestStream), context.CancellationToken);
    }

    public override async Task DownloadGdsStream(DownloadGdsStreamRequest request, IServerStreamWriter<StreamBuffer> response, ServerCallContext context)
    {
        await _mediator.Send(new DownloadGdsStreamCommand(request, response), context.CancellationToken);
    }
    #endregion
    
    public override async Task<FloorPlan> CreateFloorPlan(CreateFloorPlanRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateFloorPlanCommand(request), context.CancellationToken);
    }

    public override async Task<FloorPlan> UpdateFloorPlan(UpdateFloorPlanRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateFloorPlanCommand(request), context.CancellationToken);
    }
}
