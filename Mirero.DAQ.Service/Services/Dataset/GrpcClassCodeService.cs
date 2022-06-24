using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Dataset.Handlers.ClassCode.CreateClassCode;
using Mirero.DAQ.Application.Dataset.Handlers.ClassCode.CreateClassCodeSet;
using Mirero.DAQ.Application.Dataset.Handlers.ClassCode.DeleteClassCode;
using Mirero.DAQ.Application.Dataset.Handlers.ClassCode.DeleteClassCodeSet;
using Mirero.DAQ.Application.Dataset.Handlers.ClassCode.GetClassCode;
using Mirero.DAQ.Application.Dataset.Handlers.ClassCode.GetClassCodeSet;
using Mirero.DAQ.Application.Dataset.Handlers.ClassCode.ListClassCodes;
using Mirero.DAQ.Application.Dataset.Handlers.ClassCode.ListClassCodeSets;
using Mirero.DAQ.Application.Dataset.Handlers.ClassCode.UpdateClassCode;
using Mirero.DAQ.Application.Dataset.Handlers.ClassCode.UpdateClassCodeSet;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Services.Dataset;

public class GrpcClassCodeService : ClassCodeService.ClassCodeServiceBase
{
    private readonly ILogger<GrpcClassCodeService> _logger;
    private readonly IMediator _mediator;

    public GrpcClassCodeService(ILogger<GrpcClassCodeService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    #region ClassCodeSet
    public override async Task<ClassCodeSet> CreateClassCodeSet(CreateClassCodeSetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateClassCodeSetCommand(request), CancellationToken.None);
    }

    public override async Task<ListClassCodeSetsResponse> ListClassCodeSets(ListClassCodeSetsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListClassCodeSetsCommand(request), CancellationToken.None);
    }

    public override async Task<ClassCodeSet> GetClassCodeSet(GetClassCodeSetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetClassCodeSetCommand(request), CancellationToken.None);
    }

    public override async Task<ClassCodeSet> UpdateClassCodeSet(UpdateClassCodeSetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateClassCodeSetCommand(request), CancellationToken.None);
    }

    public override async Task<ClassCodeSet> DeleteClassCodeSet(DeleteClassCodeSetRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteClassCodeSetCommand(request), CancellationToken.None);
    }
    #endregion

    #region ClassCode
    public override async Task<ClassCode> CreateClassCode(CreateClassCodeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateClassCodeCommand(request), CancellationToken.None);
    }

    public override async Task<ListClassCodesResponse> ListClassCodes(ListClassCodesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListClassCodesCommand(request), CancellationToken.None);
    }

    public override async Task<ClassCode> GetClassCode(GetClassCodeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetClassCodeCommand(request), CancellationToken.None);
    }

    public override async Task<ClassCode> UpdateClassCode(UpdateClassCodeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateClassCodeCommand(request), CancellationToken.None);
    }

    public override async Task<ClassCode> DeleteClassCode(DeleteClassCodeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteClassCodeCommand(request), CancellationToken.None);
    }
    #endregion
}