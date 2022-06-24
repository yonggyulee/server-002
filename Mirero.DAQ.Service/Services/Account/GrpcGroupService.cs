using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Account.Handlers.Group.CreateGroup;
using Mirero.DAQ.Application.Account.Handlers.Group.CreateSystem;
using Mirero.DAQ.Application.Account.Handlers.Group.DeleteGroup;
using Mirero.DAQ.Application.Account.Handlers.Group.DeleteSystem;
using Mirero.DAQ.Application.Account.Handlers.Group.GetGroupFeatures;
using Mirero.DAQ.Application.Account.Handlers.Group.GetGroupSystems;
using Mirero.DAQ.Application.Account.Handlers.Group.ListFeatures;
using Mirero.DAQ.Application.Account.Handlers.Group.ListGroups;
using Mirero.DAQ.Application.Account.Handlers.Group.ListSystems;
using Mirero.DAQ.Application.Account.Handlers.Group.UpdateGroup;
using Mirero.DAQ.Application.Account.Handlers.Group.UpdateGroupFeatures;
using Mirero.DAQ.Application.Account.Handlers.Group.UpdateGroupSystems;
using Mirero.DAQ.Application.Account.Handlers.Group.UpdateSystem;
using Mirero.DAQ.Domain.Account.Protos.V1;
using GroupService = Mirero.DAQ.Domain.Account.Protos.V1.GroupService;

namespace Mirero.DAQ.Service.Services.Account;

public class GrpcGroupService : GroupService.GroupServiceBase
{
    private readonly ILogger<GrpcGroupService> _logger;
    private readonly IMediator _mediator;

    public GrpcGroupService(
        ILogger<GrpcGroupService> logger, 
        IMediator mediator)
    {
        _logger  = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public override async Task<ListGroupsResponse> ListGroups(ListGroupsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListGroupsCommand(request), CancellationToken.None);
    }

    public override async Task<Group> CreateGroup(CreateGroupRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateGroupCommand(request), CancellationToken.None);
    }

    public override async Task<Group> UpdateGroup(UpdateGroupRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateGroupCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> DeleteGroup(DeleteGroupRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteGroupCommand(request), CancellationToken.None);
    }

    public override async Task<Domain.Account.Protos.V1.System> CreateSystem(CreateSystemRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateSystemCommand(request), CancellationToken.None);
    }

    public override async Task<Domain.Account.Protos.V1.System> UpdateSystem(UpdateSystemRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateSystemCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> DeleteSystem(DeleteSystemRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteSystemCommand(request), CancellationToken.None);
    }

    public override async Task<GetGroupFeaturesResponse> GetGroupFeatures(GetGroupFeaturesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetGroupFeaturesCommand(request), CancellationToken.None);
    }

    public override async Task<UpdateGroupFeaturesResponse> UpdateGroupFeatures(UpdateGroupFeaturesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateGroupFeaturesCommand(request), CancellationToken.None);
    }

    public override async Task<GetGroupSystemsResponse> GetGroupSystems(GetGroupSystemsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetGroupSystemsCommand(request), CancellationToken.None);
    }

    public override async Task<UpdateGroupSystemsResponse> UpdateGroupSystems(UpdateGroupSystemsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateGroupSystemsCommand(request), CancellationToken.None);
    }

    public override async Task<ListFeaturesResponse> ListFeatures(ListFeaturesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListFeaturesCommand(request), CancellationToken.None);
    }

    public override async Task<ListSystemsResponse> ListSystems(ListSystemsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListSystemsCommand(request), CancellationToken.None);
    }
}