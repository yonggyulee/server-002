using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

public partial class AccountServiceTest
{
    [Fact]
    public async Task T02_Create_List_privilege_feature_system_group()
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
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                await _userServiceClient.CreateUserAsync(superUserRequest);
            });
        }

        Assert.True(await _fixture.SignInAsync(superUserRequest.Id, superUserRequest.Password));
        var superUserOptions = _fixture.OptionsWithAuthHeader();

        #endregion
        var listPrivilegesRequest = new ListPrivilegesRequest()
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10,
            }
        };

        var listPrivilegeResponse = _userServiceClient.ListPrivileges(listPrivilegesRequest, superUserOptions);
        Assert.NotNull(listPrivilegeResponse);
        //_output.WriteLine(listPrivilegeResponse.ToString());

        // ListFeatures
        var listFeaturesRequest = new ListFeaturesRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            }
        };

        var listFeaturesResponse = _groupServiceClient.ListFeatures(listFeaturesRequest, superUserOptions);
        Assert.NotNull(listFeaturesResponse);
        //_output.WriteLine(listFeaturesResponse.ToString());

        // ListSystems
        var listSystemRequest = new ListSystemsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            }
        };

        var listSystemResponse = _groupServiceClient.ListSystems(listSystemRequest, superUserOptions);
        Assert.NotNull(listSystemResponse);
        //_output.WriteLine(listSystemResponse.ToString());

        // ListGroups
        var listGroupRequest = new ListGroupsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            }
        };

        var listGroupResponse = _groupServiceClient.ListGroups(listGroupRequest, superUserOptions);
        Assert.NotNull(listGroupResponse);
        //_output.WriteLine(listGroupResponse.ToString());
    }
}