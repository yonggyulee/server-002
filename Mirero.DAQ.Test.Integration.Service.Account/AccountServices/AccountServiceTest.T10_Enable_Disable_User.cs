using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

public partial class AccountServiceTest
{
    [Fact]
    public async Task T10_Enable_Disable_User()
    {
        
        #region SuperAdministrator - create
        
        // SuperAdministrator
        var superUserRequest = new CreateUserRequest
        {
            Id = "Id_SuperUser",
            Name = "MIRERO",
            Password = "Mirero2816!",
            Department = "dev",
            GroupId = "MEMORY", // FOUNDRY, MEMORY, SRD, SUPER
            RoleId = "SuperAdministrator", // SuperAdministrator, GroupAdministrator, Maintainer, Developer, Reporter
        };
        
        try
        {
            _userServiceClient.CreateUser(superUserRequest);
        }
        catch
        { 
            await Assert.ThrowsAnyAsync<RpcException>(async () => { await _userServiceClient.CreateUserAsync(superUserRequest); });    
        }
        
        #endregion
        
        #region Developer - create

        //Developer
        var developerUserRequest = new CreateUserRequest
        {
            Id = "Id_DevUser",
            Name = "MIRERO",
            Password = "Mirero2816!",
            Department = "dev",
            GroupId = "MEMORY", // FOUNDRY, MEMORY, SRD, SUPER
            RoleId = "Developer", // SuperAdministrator, GroupAdministrator, Maintainer, Developer, Reporter
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
        #endregion

        //developerUser 로그인 실패 - enable : false
        var developerSignInRequest= new SignInRequest
         {
             UserId = developerUserRequest.Id,
             Password = developerUserRequest.Password,
             Lifetime = Duration.FromTimeSpan(TimeSpan.FromHours(24.0))
         };
        
        try
        {
            _signInServiceClient.SignIn(developerSignInRequest);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _signInServiceClient.SignInAsync(developerSignInRequest);
            });
        }
        
        //superUserSignIn 로그인 - enable : true
        var superUserSignInRequest= new SignInRequest
        {
            UserId = superUserRequest.Id,
            Password = superUserRequest.Password,
            Lifetime = Duration.FromTimeSpan(TimeSpan.FromHours(24.0))
        };

        try
        {
            var superUserSignInResponse = _signInServiceClient.SignIn(superUserSignInRequest);
            Assert.NotEmpty(superUserSignInResponse.ToString());
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _signInServiceClient.SignInAsync(superUserSignInRequest);
            });
        }

        Assert.True(await _fixture.SignInAsync(superUserRequest.Id, superUserRequest.Password));
        var superUserOptions = _fixture.OptionsWithAuthHeader();
        
        // superadminuser가 developeruser의 enable - true 변경해주기
        var enableUserRequest = new EnableUserRequest
        {
            UserId = developerUserRequest.Id
        };
        
        try
        {
            _userServiceClient.EnableUser(enableUserRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _userServiceClient.EnableUserAsync(enableUserRequest, superUserOptions);
            });
        }
        
        //GetUser
        var whereUserId = "Id = \"" + developerUserRequest.Id + "\"";
        var getUsersRequest = new ListUsersRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10,
                Where = whereUserId
            }
        };

        var getTrueUsersRequest = _userServiceClient.ListUsers(getUsersRequest, superUserOptions);
        Assert.True(getTrueUsersRequest.Users[0].Enabled);
        
        // developerUser 로그인 - 성공
        Assert.True(await _fixture.SignInAsync(developerUserRequest.Id, developerUserRequest.Password));
        
        // superadminuser가 developeruser의 enable - false 변경해주기
        var disableUserRequest = new DisableUserRequest
        {
            UserId = developerUserRequest.Id
        };
        
        try
        {
            _userServiceClient.DisableUser(disableUserRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _userServiceClient.DisableUserAsync(disableUserRequest, superUserOptions);
            });
        }
        
        var getFalseUserResponse = _userServiceClient.ListUsers(getUsersRequest, superUserOptions);
        Assert.False(getFalseUserResponse.Users[0].Enabled);
        
        // developerUser 로그인 - 실패
        Assert.False(await _fixture.SignInAsync(developerUserRequest.Id, developerUserRequest.Password));
    }
}