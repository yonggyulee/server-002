using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;


public partial class AccountServiceTest
{
    [Fact]
    public async Task T05_Create_SignIn_Update_Delete_User()
    {
        #region SuperAdministrator - create & signin
        
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
        
        #region Developer - create & signin

        //Developer
        var developerUserRequest = new CreateUserRequest
        {
            Id = "Id_DevUser",
            Name = "MIRERO",
            Password = "Mirero2816!",
            Department = "dev",
            GroupId = "SUPER", // FOUNDRY, MEMORY, SRD, SUPER
            RoleId = "Developer", // SuperAdministrator, GroupAdministrator, Maintainer, Developer, Reporter
            Enabled = true
        };

        try
        {
            _userServiceClient.CreateUser(developerUserRequest);
        }
        catch
        { 
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await  _userServiceClient.CreateUserAsync(developerUserRequest);;
            });    
        }

        Assert.True(await _fixture.SignInAsync(superUserRequest.Id, superUserRequest.Password));
        var developerUserOptions = _fixture.OptionsWithAuthHeader();

        #endregion
        //UpdateUser
        var updateUser = new UpdateUserRequest
        {
            Id = developerUserRequest.Id,
            Name = "미래로",
            Department = "Operation",
            RoleId = "SuperAdministrator"
        };

        try
        {
            var updatedUser = _userServiceClient.UpdateUser(updateUser, superUserOptions);
            Assert.Equal(updateUser.Department, updatedUser.Department);
            Assert.Equal(updateUser.RoleId, updatedUser.RoleId);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _userServiceClient.UpdateUserAsync(updateUser, superUserOptions);
            });   
        }
        
        //DeleteUser
        var deleteUserRequest = new DeleteUserRequest
        {
            UserId = developerUserRequest.Id
            //UserId = "fail_test"
        };
        
        //GetDeleteUser
        var whereUserId = "Id = \"" + deleteUserRequest.UserId + "\"";
        var getUsersRequest = new ListUsersRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10,
                Where = whereUserId
            }
        };

        try
        {
            _userServiceClient.DeleteUser(deleteUserRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _userServiceClient.DeleteUserAsync(deleteUserRequest, superUserOptions);
            });    
        }
        
        var deleteUserResponse = _userServiceClient.ListUsers(getUsersRequest, superUserOptions);
        Assert.Empty(deleteUserResponse.Users);
    }
}