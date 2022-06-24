using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Gds.Handlers.Volume.CreateVolume;
using Mirero.DAQ.Application.Gds.Handlers.Volume.DeleteVolume;
using Mirero.DAQ.Application.Gds.Handlers.Volume.ListVolumes;
using Mirero.DAQ.Application.Gds.Handlers.Volume.UpdateVolume;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Services.Gds
{
    public class GrpcVolumeService : VolumeService.VolumeServiceBase
    {
        private readonly ILogger<GrpcVolumeService> _logger;
        private readonly IMediator _mediator;

        public GrpcVolumeService(ILogger<GrpcVolumeService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override async Task<ListVolumesResponse> ListVolumes(ListVolumesRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new ListVolumesCommand(request), context.CancellationToken);
        }

        public override async Task<Volume> CreateVolume(CreateVolumeRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new CreateVolumeCommand(request), context.CancellationToken);
        }

        public override async Task<Volume> UpdateVolume(UpdateVolumeRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new UpdateVolumeCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> DeleteVolume(DeleteVolumeRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new DeleteVolumeCommand(request), context.CancellationToken);
        }
    }
}
