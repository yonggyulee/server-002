using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Update.Handlers.Mpp.DownloadMppStream;
using Mirero.DAQ.Application.Update.Handlers.Mpp.ListMppVersions;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Update.Protos.V1;

namespace Mirero.DAQ.Service.Services.Update;

public class GrpcMppUpdateService : MppUpdateService.MppUpdateServiceBase
{
    private readonly ILogger<GrpcMppUpdateService> _logger;
    private readonly IMediator _mediator;

    public GrpcMppUpdateService(ILogger<GrpcMppUpdateService> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public override async Task<ListMppSetupVersionsResponse> ListMppSetupVersions(ListMppSetupVersionsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListMppSetupVersionsCommand(request), context.CancellationToken);
    }

    public override async Task DownloadMppSetupVersion(DownloadMppSetupVersionRequest request, IServerStreamWriter<StreamBuffer> responseStream,
        ServerCallContext context)
    {
        await _mediator.Send(new DownloadMppSetupVersionCommand(request, responseStream), context.CancellationToken);
    }
}