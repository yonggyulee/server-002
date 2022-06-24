using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Update.Handlers.Rc.DownloadRcStream;
using Mirero.DAQ.Application.Update.Handlers.Rc.ListRcVersions;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Update.Protos.V1;
using RcUpdateService = Mirero.DAQ.Domain.Update.Protos.V1.RcUpdateService;

namespace Mirero.DAQ.Service.Services.Update;

public class GrpcRcUpdateService : RcUpdateService.RcUpdateServiceBase
{
    private readonly ILogger<GrpcRcUpdateService> _logger;
    private readonly IMediator _mediator;

    public GrpcRcUpdateService(ILogger<GrpcRcUpdateService> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public override async Task<ListRcSetupVersionsResponse> ListRcSetupVersions(ListRcSetupVersionsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListRcSetupVersionsCommand(request), CancellationToken.None);
    }

    public override async Task DownloadRcSetupVersion(DownloadRcSetupVersionRequest request, IServerStreamWriter<StreamBuffer> responseStream, ServerCallContext context)
    {
        await _mediator.Send(new DownloadRcSetupVersionCommand(request, responseStream), CancellationToken.None);
    }
}