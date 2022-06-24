using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Mirero.DAQ.Test.Custom.Sykim
{
    public class AccountTest
    {
        //private static AccountService.AccountServiceClient _accountServiceClient;
        private static Serilog.ILogger _logger = Log.Logger;

        public static void Test()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            //_accountServiceClient = new AccountService.AccountServiceClient(channel);

            // SignIn
            //TestSignIn();
            //TestSignIn2();
            //TestRefreshAccessToken();

            // Group
            // TestCreateGroup();
            // TestListGroups();
            // TestGetGroup();
            
            // TestUpdateGroup();
            //TestDeleteGroup();

            // User
            //TestListUsers();
            //TestGetUser();
            //TestCreateUser();
            //TestUpdateUser();
            //TestDeleteUser();
            //TestEnableUser();
            //TestDisableUser();

            // Role
            //TestListRoles();
            //TestGetRole();

            // Privilege - Feature - System - RolePrivilege
            //TestListPrivileges();
            //TestListFeatures();
            //TestListSystems();
            //TestListRolePrivileges();

            // UserPrivilege
            //TestListUserPrivileges();
            //TestGrantUserPrivilege();
            //TestRevokeUserPrivilege();
            //TestResetUserPrivilege(); 

            // GroupFeature
            //TestListGroupFeatures();
            //TestGrantGroupFeature();
            //TestRevokeGroupFeature();

            // GroupSystem
            //TestListGroupSystems();
            //TestAddGroupSystem();
            //TestRemoveGroupSystem();
        }

        // #region SignIn
        //
        // //로그인(SignInAsync) - TestSignIn
        // private static void TestSignIn()
        // {
        //     _logger.Information("======= 로그인 =======");
        //     var signInResponse = _accountServiceClient.SignIn(new SignInRequest
        //     {
        //         UserId = "ID_ksy01",
        //         Password = "Mirero2816!",
        //         Lifetime = Duration.FromTimeSpan(TimeSpan.FromDays(1))
        //     });
        //
        //     _logger.Information(signInResponse.ToString());
        // }
        //
        // private static void TestSignIn2()
        // {
        //     _logger.Information("======= 로그인 =======");
        //
        //     var userId = "ID_ksy2";
        //
        //     var signInResponse = _accountServiceClient.SignIn(new SignInRequest
        //     {
        //         UserId = userId,
        //         Password = "Mirero2816!",
        //         Lifetime = Duration.FromTimeSpan(TimeSpan.FromDays(1))
        //     });
        //
        //     var metadata = new Metadata();
        //     metadata.Add("Authorization", $"Bearer {signInResponse.AccessToken}");
        //
        //     /*_accountServiceClient.UpdateUser(new User
        //     {
        //         Id = userId,
        //         Password = "Mirero2816!",
        //         Department = "Operation",
        //     },metadata);*/
        //
        //     /*_accountServiceClient.UpdateGroup(new Group
        //    {
        //        Id = "FOUNDRY",
        //        Title = "FOUNDRY4",
        //    },metadata);*/
        //
        //     try
        //     {
        //         _accountServiceClient.GrantUserPrivilege(new GrantUserPrivilegeRequest
        //         {
        //             UserId = userId,
        //             PrivilegeId = "UpdateGds_new"
        //         }, metadata);
        //     }
        //     catch (RpcException ex)
        //     {
        //         _logger.Error(ex, ex.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.Error(ex, ex.Message);
        //     }
        //
        //     _logger.Information(signInResponse.ToString());
        // }
        //
        // // AccessToken을 refresh하기(RefreshAccessTokenAsync) - TestRefreshAccessToken
        // private static void TestRefreshAccessToken()
        // {
        //     _logger.Information("======= AccessToken을Refresh하기 =======");
        //     var refreshAccessTokenResponse = _accountServiceClient.RefreshAccessToken(new RefreshAccessTokenRequest
        //     {
        //         RefreshToken = "a132ce18dd434bdca06ff2861d8aba7d",
        //         Lifetime = Duration.FromTimeSpan(TimeSpan.FromDays(1))
        //     });
        //
        //     _logger.Information(refreshAccessTokenResponse.ToString());
        // }
        //
        // #endregion
        //
        // #region Group
        //
        // // 리스트그룹(ListGroupsAsync) - TestListGroups
        // private static void TestListGroups()
        // {
        //     _logger.Information("======= 리스트그룹 =======");
        //     var listGroupsResponse = _accountServiceClient.ListGroups(new ListGroupsRequest
        //     {
        //         QueryParameter = new QueryParameter
        //         {
        //             PageIndex = 0,
        //             PageSize = 3,
        //             Where = "",
        //         }
        //     });
        //
        //     _logger.Information(listGroupsResponse.ToString());
        // }
        //
        // // 1개_그룹(GetGroupAsync) - TestGetGroup
        // private static void TestGetGroup()
        // {
        //     _logger.Information("======= 1개 그룹 =======");
        //     var getGroupsResponse = _accountServiceClient.GetGroup(new GetGroupRequest
        //     {
        //         GroupId = "FOUNDRY"
        //     });
        //
        //     _logger.Information(getGroupsResponse.ToString());
        // }
        //
        // // 그룹생성(CreateGroupAsync) - TestCreateGroup
        // private static void TestCreateGroup()
        // {
        //     _logger.Information("======= 그룹생성 =======");
        //     var group = _accountServiceClient.CreateGroup(new Group
        //     {
        //         Id = "New_GROUP1",
        //         Title = "New_Group1"
        //     });
        //
        //     _logger.Information(group.ToString());
        // }
        //
        // //업데이트그룹(UpdateGroupAsync) - TestUpdateGroup
        // private static void TestUpdateGroup()
        // {
        //     _logger.Information("======= 업데이트그룹 =======");
        //     var updateGroupResponse = _accountServiceClient.UpdateGroup(new Group
        //     {
        //         Id = "FOUNDRY",
        //         Title = "FOUNDRY",
        //     });
        //
        //     _logger.Information(updateGroupResponse.ToString());
        // }
        //
        // // region 삭제그룹(DeleteGroupAsync) - TestDeleteGroup
        // private static void TestDeleteGroup()
        // {
        //     Console.WriteLine("======= 삭제그룹 =======");
        //     var DeleteGroupResponse = _accountServiceClient.DeleteGroup(new DeleteGroupRequest
        //     {
        //         GroupId = "FOUNDRY"
        //     });
        //
        //     _logger.Information(DeleteGroupResponse.ToString());
        // }
        //
        // #endregion
        //
        // #region User
        //
        // // 리스트유저(ListUsersAsync) - TestListUsers
        // private static void TestListUsers()
        // {
        //     _logger.Information("======= 리스트유저 =======");
        //     var listUsersResponse = _accountServiceClient.ListUsers(new ListUsersRequest
        //     {
        //         QueryParameter = new QueryParameter
        //         {
        //             PageIndex = 0,
        //             PageSize = 3,
        //             Where = "",
        //         }
        //     });
        //
        //     _logger.Information(listUsersResponse.ToString());
        // }
        //
        // // 1명_유저(GetUserAsync) - TestGetUser
        // private static void TestGetUser()
        // {
        //     _logger.Information("======= 1명유저 =======");
        //     var getUsersResponse = _accountServiceClient.GetUser(new GetUserRequest
        //     {
        //         UserId = "ID_ksy1"
        //     });
        //
        //     _logger.Information(getUsersResponse.ToString());
        // }
        //
        // // 회원가입(CreateUserAsync) - TestCreateUser
        // private static void TestCreateUser()
        // {
        //     _logger.Information("======= 회원가입 =======");
        //     var lifetime = new Duration
        //     {
        //         Seconds = 10000
        //     };
        //
        //     try
        //     {
        //         var user = _accountServiceClient.CreateUser(new User
        //         {
        //             Id = "ID_ksy05",
        //             Name = "김순영",
        //             Password = "Mirero2816!",
        //             Department = "dev",
        //             GroupId = "MEMORY", // FOUNDRY, MEMORY, SRD, SUPER
        //             RoleId = "Maintainer" // SuperAdministrator, Maintainer, GroupAdministrator 
        //         });
        //
        //         _logger.Information(user.ToString());
        //     }
        //     catch (RpcException ex)
        //     {
        //         _logger.Error(ex, ex.Message);
        //     }
        // }
        //
        //
        // // 업데이트유저(UpdateUserAsync)- TestUpdateUser
        // private static void TestUpdateUser()
        // {
        //     _logger.Information("======= 업데이트유저 =======");
        //     var updateGroupResponse = _accountServiceClient.UpdateUser(new User
        //     {
        //         Id = "ID_ksy4",
        //         Password = "Mirero2816!",
        //         Department = "Operation",
        //     });
        //
        //     _logger.Information(updateGroupResponse.ToString());
        // }
        //
        // // 삭제유저(DeleteUserAsync) - TestDeleteUser
        // private static void TestDeleteUser()
        // {
        //     _logger.Information("======= 삭제유저 =======");
        //     var deleteUserResponse = _accountServiceClient.DeleteUser(new DeleteUserRequest
        //     {
        //         UserId = "ID_ksy10"
        //     });
        //
        //     _logger.Information(deleteUserResponse.ToString());
        // }
        //
        // // Enable유저(EnableUserAsync) - TestEnableUser
        // private static void TestEnableUser()
        // {
        //     _logger.Information("======= Enable유저 =======");
        //     var enableUserResponse = _accountServiceClient.EnableUser(new EnableUserRequest
        //     {
        //         UserId = "ID_ksy3"
        //     });
        //
        //     _logger.Information(enableUserResponse.ToString());
        // }
        //
        // // Disable유저(DisableUserAsync) - TestDisableUser
        //
        // private static void TestDisableUser()
        // {
        //     _logger.Information("======= Disable유저 =======");
        //     var disableUserResponse = _accountServiceClient.DisableUser(new DisableUserRequest
        //     {
        //         UserId = "ID_ksy3"
        //     });
        //
        //     _logger.Information(disableUserResponse.ToString());
        // }
        //
        // #endregion
        //
        // #region Role
        //
        // //리스트Role(ListRolesAsync) - TestListRoles
        // private static void TestListRoles()
        // {
        //     _logger.Information("======= 리스트Role =======");
        //     var listRoleResponse = _accountServiceClient.ListRoles(new ListRolesRequest
        //     {
        //         QueryParameter = new QueryParameter
        //         {
        //             PageIndex = 0,
        //             PageSize = 3,
        //             Where = "",
        //         }
        //     });
        //
        //     _logger.Information(listRoleResponse.ToString());
        // }
        //
        // //1개_Role(GetRoleAsync) - TestGetRole
        // private static void TestGetRole()
        // {
        //     _logger.Information("======= 1개 Role =======");
        //     var getRoleResponse = _accountServiceClient.GetRole(new GetRoleRequest
        //     {
        //         RoleId = "SuperAdministrator"
        //     });
        //
        //     _logger.Information(getRoleResponse.ToString());
        // }
        //
        // #endregion
        //
        // #region Privilege - Feature - System - RolePrivilege
        //
        // // 리스트Privileges(ListPrivilegesAsync) - TestListPrivileges
        // private static void TestListPrivileges()
        // {
        //     _logger.Information("======= 리스트Privileges =======");
        //     var listPrivilegesResponse = _accountServiceClient.ListPrivileges(new ListPrivilegesRequest
        //     {
        //         QueryParameter = new QueryParameter
        //         {
        //             PageIndex = 0,
        //             PageSize = 3,
        //             Where = "",
        //         }
        //     });
        //
        //     _logger.Information(listPrivilegesResponse.ToString());
        // }
        //
        // // 리스트Features(ListFeaturesAsync) - TestListFeatures
        // private static void TestListFeatures()
        // {
        //     _logger.Information("======= 리스트Features =======");
        //
        //     var listFeaturesResponse = _accountServiceClient.ListFeatures(new ListFeaturesRequest
        //     {
        //         QueryParameter = new QueryParameter
        //         {
        //             PageIndex = 0,
        //             PageSize = 1,
        //             Where = "",
        //         }
        //     });
        //
        //     _logger.Information(listFeaturesResponse.ToString());
        // }
        //
        // // 리스트Features(ListSystemsAsync) - TestListSystems
        // private static void TestListSystems()
        // {
        //     _logger.Information("======= 리스트Systems =======");
        //     var listSystemsResponse = _accountServiceClient.ListSystems(new ListSystemsRequest
        //     {
        //         QueryParameter = new QueryParameter
        //         {
        //             PageIndex = 0,
        //             PageSize = 3,
        //             Where = "",
        //         }
        //     });
        //
        //     _logger.Information(listSystemsResponse.ToString());
        // }
        //
        // //리스트RolePrivileges(ListRolePrivilegesAsync) - TestListRolePrivileges
        // private static void TestListRolePrivileges()
        // {
        //     _logger.Information("======= 리스트RolePrivileges =======");
        //     var listRolePrivilegesResponse = _accountServiceClient.ListRolePrivileges(new ListRolePrivilegesRequest
        //     {
        //         QueryParameter = new QueryParameter
        //         {
        //             PageIndex = 0,
        //             PageSize = 3,
        //             Where = "",
        //         },
        //         RoleId = "SUPER_ADMINISTRATOR"
        //     });
        //
        //     _logger.Information(listRolePrivilegesResponse.ToString());
        // }
        //
        // #endregion
        //
        // #region UserPrivilege
        //
        // //리스트UserPrivileges(ListUserPrivilegesAsync) - TestListUserPrivileges
        // private static void TestListUserPrivileges()
        // {
        //     _logger.Information("======= 리스트UserPrivileges =======");
        //     var listUserPrivilegesResponse = _accountServiceClient.ListUserPrivileges(new ListUserPrivilegesRequest
        //     {
        //         QueryParameter = new QueryParameter
        //         {
        //             PageIndex = 0,
        //             PageSize = 40,
        //             Where = "",
        //         },
        //         UserId = "ID_ksy3"
        //     });
        //
        //     _logger.Information(listUserPrivilegesResponse.ToString());
        // }
        //
        // //부여UserPrivilege(GrantUserPrivilegeAsync) - TestGrantUserPrivilege
        // private static void TestGrantUserPrivilege()
        // {
        //     _logger.Information("======= 부여UserPrivilege =======");
        //     _accountServiceClient.GrantUserPrivilege(new GrantUserPrivilegeRequest
        //     {
        //         UserId = "ID_ksy2",
        //         PrivilegeId = "UpdateGds"
        //     });
        //
        //     _logger.Information("사용자에게 권한을 부여했습니다.");
        // }
        //
        // //취소UserPrivileg(RevokeUserPrivilegeAsync)- TestRevokeUserPrivilege
        // private static void TestRevokeUserPrivilege()
        // {
        //     _logger.Information("======= 취소UserPrivileg =======");
        //     _accountServiceClient.RevokeUserPrivilege(new RevokeUserPrivilegeRequest
        //     {
        //         UserId = "ID_ksy2",
        //         PrivilegeId = "DeleteRecipe"
        //     });
        //
        //     _logger.Information("사용자에게서 권한을 철회하였습니다.");
        // }
        //
        //
        // //region 초기화UserPrivilege(ResetUserPrivilegeAsync) - TestResetUserPrivilege
        //
        // private static void TestResetUserPrivilege()
        // {
        //     _logger.Information("======= 초기화UserPrivilege =======");
        //     _accountServiceClient.ResetUserPrivilege(new ResetUserPrivilegeRequest
        //     {
        //         UserId = "ID_ksy2"
        //     });
        //
        //     _logger.Information("사용자에게서 권한을 초기화하였습니다.");
        // }
        //
        // #endregion
        //
        // #region GroupFeature
        //
        // //region 리스트GroupFeatures(ListGroupFeaturesAsync) - TestListGroupFeatures
        //
        // private static void TestListGroupFeatures()
        // {
        //     _logger.Information("======= 리스트GroupFeatures =======");
        //     var listGroupFeaturesResponse = _accountServiceClient.ListGroupFeatures(new ListGroupFeaturesRequest
        //     {
        //         GroupId = "MEMORY",
        //         QueryParameter = new QueryParameter
        //         {
        //             PageIndex = 0,
        //             PageSize = 20,
        //             Where = "",
        //         }
        //     });
        //
        //     _logger.Information(listGroupFeaturesResponse.ToString());
        // }
        //
        // //부여GroupFeature(GrantGroupFeatureAsync) - TestGrantGroupFeature
        // private static void TestGrantGroupFeature()
        // {
        //     _logger.Information("======= 부여GroupFeature =======");
        //     _accountServiceClient.GrantGroupFeature(new GrantGroupFeatureRequest
        //     {
        //         GroupId = "MEMORY",
        //         FeatureId = "RecipeMonitoring"
        //     });
        //
        //     _logger.Information("Group에게 Feature를 부여했습니다.");
        // }
        //
        //
        // // 취소RevokeGroupFeature(RevokeGroupFeatureAsync) - TestRevokeGroupFeature
        // private static void TestRevokeGroupFeature()
        // {
        //     _logger.Information("======= 취소RevokeGroupFeature =======");
        //     _accountServiceClient.RevokeGroupFeature(new RevokeGroupFeatureRequest
        //     {
        //         GroupId = "MEMORY",
        //         FeatureId = "RecipeDevelopment"
        //     });
        //
        //     _logger.Information("Group에게서 Feature를 철회했습니다.");
        // }
        //
        // #endregion
        //
        // #region GroupSystem
        //
        // private static void TestListGroupSystems()
        // {
        //     _logger.Information("======= 리스트GroupSystems =======");
        //     var ListGroupSystemsResponse = _accountServiceClient.ListGroupSystems(new ListGroupSystemsRequest
        //     {
        //         GroupId = "MEMORY",
        //         QueryParameter = new QueryParameter
        //         {
        //             PageIndex = 0,
        //             PageSize = 3,
        //             Where = "",
        //         }
        //     });
        //
        //     _logger.Information(ListGroupSystemsResponse.ToString());
        // }
        //
        // private static void TestAddGroupSystem()
        // {
        //     _logger.Information("======= 추가GroupSystems =======");
        //     _accountServiceClient.AddGroupSystem(new AddGroupSystemRequest
        //     {
        //         GroupId = "SRD",
        //         SystemId = "NRDV_INLINE"
        //     });
        //
        //     _logger.Information("Group에게 System이 추가되었습니다.");
        // }
        //
        // private static void TestRemoveGroupSystem()
        // {
        //     _logger.Information("======= 삭제GroupSystems =======");
        //     _accountServiceClient.RemoveGroupSystem(new RemoveGroupSystemRequest
        //     {
        //         GroupId = "SRD",
        //         SystemId = "NRDV_INLINE"
        //     });
        //
        //     _logger.Information("Group에서 System이 삭제되었습니다.");
        // }
        //
        // #endregion
    }
}