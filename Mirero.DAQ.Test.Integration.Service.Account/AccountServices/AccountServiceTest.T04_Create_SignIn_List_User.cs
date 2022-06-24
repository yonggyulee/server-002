using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

public partial class AccountServiceTest
{
    [Fact]
    public async Task T04_Create_SignIn_List_User()
    {
        #region SuperAdministrator - create & signin
        
        // SuperAdministrator
        var superUserRequest = new CreateUserRequest
        {
            Id = "Id_SuperUser",
            Name = "MIRERO",
            Password = "Mirero2816!",
            //Department = "",
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
            //Department = "",
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
        
        var listUsersRequest = new ListUsersRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 1
            }
        };
        
        var listUsersResponse = _userServiceClient.ListUsers(listUsersRequest, superUserOptions);
        _output.WriteLine(listUsersResponse.ToString());
        Assert.NotNull(listUsersResponse);
    }
}