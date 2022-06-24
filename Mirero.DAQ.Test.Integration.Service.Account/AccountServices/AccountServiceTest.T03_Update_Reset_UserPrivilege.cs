using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

public partial class AccountServiceTest
{
    [Fact]
    public async Task T03_Update_Reset_UserPrivilege()
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
        
        var whereUserId = "Id = \"" + superUserRequest.Id + "\"";
        var getUserRequest = new ListUsersRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10,
                Where = whereUserId
            }
        };

        var getSuperUser = _userServiceClient.ListUsers(getUserRequest, superUserOptions);
        Assert.Equal(superUserRequest.Id, getSuperUser.Users[0].Id);
        
        // UpdateUserPrivilege
        var updateUserPrivilegeRequest = new UpdateUserPrivilegeRequest
        {
            UserId = getSuperUser.Users[0].Id,
            Privileges = { 
                new Privilege
                {
                    Id = "DeleteUser",
                    Title = "DeleteUser",
                    Enabled = true,
                    Category = "test"
                }, 
                new Privilege
                {
                    Id = "CreateGroup",
                    Title = "CreateGroup",
                    Enabled = true,
                    Category = "test"
                },
                new Privilege
                {
                    Id = "DeleteGroup",
                    Title = "DeleteGroup",
                    Enabled = false,
                    Category = "test"
                }
            }
        };
        
        var updateUserPrivilegeResponse=  _userServiceClient.UpdateUserPrivilege(updateUserPrivilegeRequest, superUserOptions);
        Assert.Equal(updateUserPrivilegeRequest.Privileges.Count, updateUserPrivilegeResponse.Privileges.Count);
      
       // ResetUserPrivilege
       var resetUserPrivilegeRequest = new ResetUserPrivilegeRequest
       {
           UserId = superUserRequest.Id
       };

       var resetUserPrivilegeResponse = _userServiceClient.ResetUserPrivilege(resetUserPrivilegeRequest, superUserOptions);
        //Assert.Empty(resetUserPrivilegeResponse.ToString());
        Assert.Equal(resetUserPrivilegeResponse, new Empty());
    }
}
