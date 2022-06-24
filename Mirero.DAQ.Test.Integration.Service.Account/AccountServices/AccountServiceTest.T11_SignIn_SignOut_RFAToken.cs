using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

public partial class AccountServiceTest
{
    private SignInResponse superUserSignInResponse { get; set; }
    
    [Fact]
    public async Task T11_SignIn_SignOut_RFAToken()
    {
        var options = new CallOptions();
        
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
        
        // SignIn
        var superUserSignInRequest= new SignInRequest
        {
            UserId = superUserRequest.Id,
            Password = superUserRequest.Password,
            Lifetime = Duration.FromTimeSpan(TimeSpan.FromHours(24.0))
        };
        
        try
        {
            superUserSignInResponse = _signInServiceClient.SignIn(superUserSignInRequest);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _signInServiceClient.SignInAsync(superUserSignInRequest);
            });
        }
        Assert.True(await _fixture.SignInAsync(superUserRequest.Id, superUserRequest.Password));
        
        var headers = new Metadata()
        {
            { "Authorization", $"Bearer {superUserSignInResponse.AccessToken}" }
        };
        var superUserOptions = options.WithHeaders(headers);
        
       // RefreshAccessToken
       var refreshAccessTokenRequest = new RefreshAccessTokenRequest
       {
           RefreshToken = superUserSignInResponse.RefreshToken,
           Lifetime = Duration.FromTimeSpan(TimeSpan.FromHours(24.0))
       };
       
       try
       {
           _signInServiceClient.RefreshAccessToken(refreshAccessTokenRequest, superUserOptions);
       }
       catch
       {
           await Assert.ThrowsAnyAsync<RpcException>(async () =>
           {
               await _signInServiceClient.RefreshAccessTokenAsync(refreshAccessTokenRequest, superUserOptions);
           });
       }
       
       // SignOut
       var signOutRequest = new SignOutRequest
       {
           UserId = superUserRequest.Id,
       };
       
       try
       {
           _signInServiceClient.SignOut(signOutRequest, superUserOptions);
       }
       catch
       {
           await Assert.ThrowsAnyAsync<RpcException>(async () =>
           {
               await _signInServiceClient.SignOutAsync(signOutRequest, superUserOptions);
           });
       }
    }
}