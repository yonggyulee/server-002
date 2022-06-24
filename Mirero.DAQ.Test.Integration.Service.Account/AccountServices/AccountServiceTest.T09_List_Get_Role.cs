using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

public partial class AccountServiceTest
{
    [Fact]
    public async Task T09_List_Get_Role()
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
        
        // list와 get의 role response에 properties/description가 있는데, 이거에 들어가는 값이 어떻게 채워지는가?
        var listRolesRequest = new ListRolesRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10,
            }
        };
        
        try
        {
            var listRolesResponse = _userServiceClient.ListRoles(listRolesRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _userServiceClient.ListRolesAsync(listRolesRequest, superUserOptions);
            });    
        }
        
        var getRoleRequest = new GetRoleRequest
        {
            RoleId = "GroupAdministrator"
        };
        
        try
        { 
            _userServiceClient.GetRole(getRoleRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _userServiceClient.GetRoleAsync(getRoleRequest, superUserOptions);
            });    
        }
    }
}