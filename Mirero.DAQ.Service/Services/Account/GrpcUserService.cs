using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Account.Handlers.User.CreateUser;
using Mirero.DAQ.Application.Account.Handlers.User.DeleteUser;
using Mirero.DAQ.Application.Account.Handlers.User.DisableUser;
using Mirero.DAQ.Application.Account.Handlers.User.EnableUser;
using Mirero.DAQ.Application.Account.Handlers.User.GetRole;
using Mirero.DAQ.Application.Account.Handlers.User.ListPrivileges;
using Mirero.DAQ.Application.Account.Handlers.User.ListRoles;
using Mirero.DAQ.Application.Account.Handlers.User.ListUsers;
using Mirero.DAQ.Application.Account.Handlers.User.ResetUserPrivilege;
using Mirero.DAQ.Application.Account.Handlers.User.UpdateUser;
using Mirero.DAQ.Application.Account.Handlers.User.UpdateUserPrivilege;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Serilog;
using UserService = Mirero.DAQ.Domain.Account.Protos.V1.UserService;

namespace Mirero.DAQ.Service.Services.Account;

public class GrpcUserService : UserService.UserServiceBase
{
    private readonly ILogger<GrpcUserService> _logger;
    private readonly IMediator _mediator;


    public GrpcUserService(ILogger<GrpcUserService> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public override async Task<ListUsersResponse> ListUsers(ListUsersRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListUsersCommand(request), CancellationToken.None);
    }

    public override async Task<User> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateUserCommand(request), CancellationToken.None);
    }

    public override async Task<User> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateUserCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DeleteUserCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> EnableUser(EnableUserRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new EnableUserCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> DisableUser(DisableUserRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new DisableUserCommand(request), CancellationToken.None);
    }

    public override async Task<ListRolesResponse> ListRoles(ListRolesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListRolesCommand(request), CancellationToken.None);
    }
    
    public override async Task<GetRoleResponse> GetRole(GetRoleRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new GetRoleCommand(request), CancellationToken.None);
    }

    public override async Task<UpdateUserPrivilegeResponse> UpdateUserPrivilege(UpdateUserPrivilegeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new UpdateUserPrivilegeCommand(request), CancellationToken.None);
    }

    public override async Task<Empty> ResetUserPrivilege(ResetUserPrivilegeRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ResetUserPrivilegeCommand(request), CancellationToken.None);
    }

    public override async Task<ListPrivilegesResponse> ListPrivileges(ListPrivilegesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new ListPrivilegesCommand(request), CancellationToken.None);
    }
}