using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.Handlers.Volume.CreateVolume;
using Mirero.DAQ.Application.Inference.Handlers.Volume.DeleteVolume;
using Mirero.DAQ.Application.Inference.Handlers.Volume.ListVolumes;
using Mirero.DAQ.Application.Inference.Handlers.Volume.UpdateVolume;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Service.Services.Inference;

public class GrpcVolumeService : VolumeService.VolumeServiceBase
{
    private readonly ILogger<GrpcInferenceService> _logger;
    private readonly IMediator _mediator;

    public GrpcVolumeService(ILogger<GrpcInferenceService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task<ListVolumesResponse> ListVolumes(ListVolumesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListVolumesCommand(request), CancellationToken.None);
    }

    public override async Task<Volume> CreateVolume(CreateVolumeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateVolumeCommand(request), CancellationToken.None);
    }

    public override async Task<Volume> UpdateVolume(UpdateVolumeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateVolumeCommand(request), CancellationToken.None);
    }

    public override async Task<Volume> DeleteVolume(DeleteVolumeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteVolumeCommand(request), CancellationToken.None);
    }
}