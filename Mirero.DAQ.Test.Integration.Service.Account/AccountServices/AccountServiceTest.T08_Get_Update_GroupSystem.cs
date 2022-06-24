using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Xunit;
using System = Mirero.DAQ.Domain.Account.Protos.V1.System;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

public partial class AccountServiceTest
{
    [Fact]
    public async Task T08_Get_Update_GroupSystem()
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

        var getGroupSystemsRequest = new GetGroupSystemsRequest
        {
            GroupId = "MEMORY",
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10,
            }
        };

        try
        {
            _groupServiceClient.GetGroupSystems(getGroupSystemsRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _groupServiceClient.GetGroupSystemsAsync(getGroupSystemsRequest, superUserOptions);
            });    
        }

        var updateGroupSystemsRequest = new UpdateGroupSystemsRequest
        {
            GroupId = "MEMORY",
            Systems =
            {
                new Domain.Account.Protos.V1.System
                {
                    Id = "16_INLINE",
                    Title = "16_INLINE_update",
                },
            }
        };

        try
        {
            var updateGroupSystemsResponse= _groupServiceClient.UpdateGroupSystems(updateGroupSystemsRequest, superUserOptions);
            Assert.Equal(updateGroupSystemsRequest.Systems.Count, updateGroupSystemsResponse.Systems.Count);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _groupServiceClient.UpdateGroupSystemsAsync(updateGroupSystemsRequest, superUserOptions);
            });    
        }
    }
}