using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

public partial class AccountServiceTest
{
    [Fact]
    public async Task T01_Create_Get_Update_Delete_Group()
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
            var superUser = _userServiceClient.CreateUser(superUserRequest);
        }
        catch
        { 
            await Assert.ThrowsAnyAsync<RpcException>(async () => { await _userServiceClient.CreateUserAsync(superUserRequest); });    
        }

        Assert.True(await _fixture.SignInAsync(superUserRequest.Id, superUserRequest.Password));
        var superUserOptions = _fixture.OptionsWithAuthHeader();

        #endregion

        var creatGroupRequest = new CreateGroupRequest
        {
            Id = "Id_Group1", // TODO 클라이언트에서 Id를 정말 줘야 하는지 확인 요망
            Title = "Title_Group1",
            Description = "Group1 "
        };
        
        try
        {
            var group = _groupServiceClient.CreateGroup(creatGroupRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _groupServiceClient.CreateGroupAsync(creatGroupRequest, superUserOptions);
            });
        }

        var whereGroupId = "Id = \"" + creatGroupRequest.Id + "\"";
        
        var getGroupRequest = new ListGroupsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10,
                Where = whereGroupId
            }
        };
        
        var createGroupBack = _groupServiceClient.ListGroups(getGroupRequest, superUserOptions);

        Assert.Equal(creatGroupRequest.Description, createGroupBack.Groups[0].Description);
        Assert.Equal(creatGroupRequest.Title, createGroupBack.Groups[0].Title);
        
        var updateGroupRequest = new UpdateGroupRequest
        {
            Id = createGroupBack.Groups[0].Id,
            Title = createGroupBack.Groups[0].Title+"_update"
        };

        try
        {
            await _groupServiceClient.UpdateGroupAsync(updateGroupRequest, superUserOptions);
            //TODO check `group` is update     
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _groupServiceClient.UpdateGroupAsync(updateGroupRequest, superUserOptions);
            });
            //TODO check `group` is not update  -> fail(Group ID Doesn't exist)
        }
        
        var updateGroupBack = _groupServiceClient.ListGroups(getGroupRequest, superUserOptions);
        
        Assert.Equal(updateGroupRequest.Title, updateGroupBack.Groups[0].Title);

        
        var deleteGroupRequest = new DeleteGroupRequest
        {
            GroupId = creatGroupRequest.Id
        };

        try
        {
            _groupServiceClient.DeleteGroup(deleteGroupRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _groupServiceClient.DeleteGroupAsync(deleteGroupRequest, superUserOptions);
            });
        }

        var deleteGroupBack = _groupServiceClient.ListGroups(getGroupRequest, superUserOptions);
        Assert.Empty(deleteGroupBack.Groups);
    }
}