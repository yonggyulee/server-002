using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Dataset.Handlers.Volume.CreateVolume;
using Mirero.DAQ.Application.Dataset.Handlers.Volume.DeleteVolume;
using Mirero.DAQ.Application.Dataset.Handlers.Volume.GetVolume;
using Mirero.DAQ.Application.Dataset.Handlers.Volume.ListVolumes;
using Mirero.DAQ.Application.Dataset.Handlers.Volume.UpdateVolume;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Services.Dataset;

public class GrpcVolumeService : VolumeService.VolumeServiceBase
{
    private readonly ILogger<GrpcVolumeService> _logger;
    private readonly IMediator _mediator;

    public GrpcVolumeService(ILogger<GrpcVolumeService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region Volume
    public override async Task<Volume> CreateVolume(CreateVolumeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateVolumeCommand(request), CancellationToken.None);
    }

    public override async Task<ListVolumesResponse> ListVolumes(ListVolumesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListVolumesCommand(request), CancellationToken.None);
    }

    public override async Task<Volume> GetVolume(GetVolumeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetVolumeCommand(request), CancellationToken.None);
    }

    public override async Task<Volume> UpdateVolume(UpdateVolumeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateVolumeCommand(request), CancellationToken.None);
    }

    public override async Task<Volume> DeleteVolume(DeleteVolumeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteVolumeCommand(request), CancellationToken.None);
    }
    #endregion
}