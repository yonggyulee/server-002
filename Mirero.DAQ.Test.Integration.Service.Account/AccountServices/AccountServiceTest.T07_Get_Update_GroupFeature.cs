using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

public partial class AccountServiceTest
{
    [Fact]
    public async Task T07_Get_Update_GroupFeature()
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
        
        // crateGroupFeature가 별도로 없어서 db에 있는 데이터로 조회
        var getGroupFeaturesRequest = new GetGroupFeaturesRequest
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
            _groupServiceClient.GetGroupFeatures(getGroupFeaturesRequest, superUserOptions);
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _groupServiceClient.GetGroupFeaturesAsync(getGroupFeaturesRequest, superUserOptions);
            });    
        }
        
        var groupFeaturesResponse = _groupServiceClient.GetGroupFeatures(getGroupFeaturesRequest, superUserOptions);
        Assert.NotEmpty(groupFeaturesResponse.Features);
        
        var updateGroupFeaturesRequest = new UpdateGroupFeaturesRequest
        {
            GroupId = "MEMORY",
            Features =
            {
                new Feature
                {
                    Id = "PipelineDevelopment",
                    Title = "PipelineDevelopment",
                    Enabled = true,
                    Category = "test"
                },
                new Feature
                {
                    Id = "PipelineMonitoring",
                    Title = "PipelineMonitoring",
                    Enabled = true,
                    Category = "test"
                },
                new Feature
                {
                    Id = "GdsManagement",
                    Title = "GdsManagement",
                    Enabled = false,
                    Category = "test"
                }

            }
        };

        try
        {
            var updateGroupFeaturesResponse = _groupServiceClient.UpdateGroupFeatures(updateGroupFeaturesRequest, superUserOptions);
            Assert.Equal(updateGroupFeaturesRequest.Features.Count(f => f.Enabled == true), updateGroupFeaturesResponse.Features.Count(f => f.Enabled == true));
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _groupServiceClient.UpdateGroupFeaturesAsync(updateGroupFeaturesRequest, superUserOptions);
            });    
        }
    }
}
