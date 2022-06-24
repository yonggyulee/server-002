using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

public partial class AccountServiceTest
{
    [Fact]
    public async Task T06_Create_Update_Delete_System()
    {
        #region create & signin

        // SuperAdministrator
        var superUserRequest = new CreateUserRequest
        {
            Id = "Id_SuperUser",
            Name = "MIRERO",
            Password = "Mirero2816!",
            Department = "dev",
            GroupId = "MEMORY", // FOUNDRY, MEMORY, SRD, SUPER
            RoleId = "SuperAdministrator", // SuperAdministrator, GroupAdministrator, Maintainer, Developer, Reporter
            Enabled = true
        };
        
        try
        {
            _userServiceClient.CreateUser(superUserRequest);
        }
        catch
        { 
            await Assert.ThrowsAnyAsync<RpcException>(async () => { await _userServiceClient.CreateUserAsync(superUserRequest); });    
        }

        Assert.True(await _fixture.SignInAsync(superUserRequest.Id, superUserRequest.Password));
        var superUserOptions = _fixture.OptionsWithAuthHeader();

        #endregion

        var createSystemRequest = new CreateSystemRequest
        {
            Id = "18_INLINE",
            Title = "18_INLINE",
            Description = "DESC",
        };

        try
        {
           _groupServiceClient.CreateSystem(createSystemRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _groupServiceClient.CreateSystemAsync(createSystemRequest, superUserOptions);
            });    
        }
        
        var whereSystemId = "Id = \"" + createSystemRequest.Id + "\"";
        var listSystemsRequest = new ListSystemsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10,
                Where = whereSystemId
            }
        };

        var listGroupResponse = _groupServiceClient.ListSystems(listSystemsRequest, superUserOptions);
        Assert.Equal(createSystemRequest.Id, listGroupResponse.Systems[0].Id);

        var updateSystemRequest = new UpdateSystemRequest
        {
            Id = "18_INLINE",
            Title = "18_INLINE_update",
            Description = "DESC",
        };

        try
        { 
            _groupServiceClient.UpdateSystem(updateSystemRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _groupServiceClient.UpdateSystemAsync(updateSystemRequest, superUserOptions);
            });    
        }
        
        var updateGroupResponse = _groupServiceClient.ListSystems(listSystemsRequest, superUserOptions);
        Assert.Equal(updateSystemRequest.Title, updateGroupResponse.Systems[0].Title);

        var deleteSystemRequest = new DeleteSystemRequest
        {
            SystemId = "18_INLINE"
        };
        
        try
        {
            _groupServiceClient.DeleteSystem(deleteSystemRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _groupServiceClient.DeleteSystemAsync(deleteSystemRequest, superUserOptions);
            });    
        }
        
        var deleteGroupResponse = _groupServiceClient.ListSystems(listSystemsRequest, superUserOptions);
        Assert.Empty(deleteGroupResponse.Systems);
    }
}
