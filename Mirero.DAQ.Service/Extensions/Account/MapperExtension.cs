using System.Collections;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Mapster;
using Mirero.DAQ.Domain.Account.Entities;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Protos;
using GroupEntity = Mirero.DAQ.Domain.Account.Entities.Group;
using GroupDto = Mirero.DAQ.Domain.Account.Protos.V1.Group;
using UserEntity = Mirero.DAQ.Domain.Account.Entities.User;
using UserDto = Mirero.DAQ.Domain.Account.Protos.V1.User;
using PrivilegesEntity = Mirero.DAQ.Domain.Account.Entities.Privilege;
using PrivilegesDto = Mirero.DAQ.Domain.Account.Protos.V1.Privilege;
using RoleDto = Mirero.DAQ.Domain.Account.Protos.V1.Role;
using FeatureDto = Mirero.DAQ.Domain.Account.Protos.V1.Feature;
using FeatureEntity = Mirero.DAQ.Domain.Account.Entities.Feature;
using SystemDto = Mirero.DAQ.Domain.Account.Protos.V1.System;
using SystemEntity = Mirero.DAQ.Domain.Account.Entities.System;
using UserPrivilegeEntity = Mirero.DAQ.Domain.Account.Entities.UserPrivilege;
using GroupFeatureEntity = Mirero.DAQ.Domain.Account.Entities.GroupFeature;
using GroupSystemEntity = Mirero.DAQ.Domain.Account.Entities.GroupSystem;

namespace Mirero.DAQ.Service.Extensions.Account;

public static class MapperExtension
{
    public static TypeAdapterConfig AddAccountMapperConfig(this TypeAdapterConfig config)
    {
        _AddGroupMapperConfig(config);
        _AddUserMapperConfig(config);
        _AddSystemMapperConfig(config);
        _AddFeatureMapperConfig(config);
        _AddPrivilegesMapperConfig(config);
        _AddRoleMapperConfig(config);
        _AddUserPrivilegeMapperConfig(config);
        _AddGroupFeatureMapperConfig(config);
        _AddGroupSystemMapperConfig(config);
        
        return config;
    }
    
    private static void _AddGroupMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<GroupEntity, GroupDto>();

        config.NewConfig<(
                ListGroupsRequest request,
                IEnumerable<GroupDto> groups,
                int count), ListGroupsResponse>()
            .ConstructUsing(src => new ListGroupsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                Groups = { src.groups }
            });
        
        config.NewConfig<(string Id, string Title), GroupDto>()
            .ConstructUsing(src => new GroupDto
            {
                Id = src.Id,
                Title = src.Title,
            });
        
        config.NewConfig<(
                ListGroupsRequest request,
                IEnumerable<GroupDto> groups,
                int count), ListGroupsResponse>()
            .ConstructUsing(src => new ListGroupsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                Groups = { src.groups }
            });
    }
    
    private static void _AddUserMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<UserEntity, UserDto>();
        
        config.NewConfig<(ListUsersRequest request,
                IEnumerable<UserDto> users,
                int count), ListUsersResponse>()
            .ConstructUsing(src => new ListUsersResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                Users = { src.users }
            });
        
        config.NewConfig<(ListUsersRequest request,
                List<UserDto> users,
                int count), ListUsersResponse>()
            .ConstructUsing(src => new ListUsersResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                Users = { src.users }
            });
        
        config.NewConfig<IEnumerable<PrivilegesDto>, UpdateUserPrivilegeResponse>()
            .ConstructUsing(src => new UpdateUserPrivilegeResponse
            {
                Privileges = { src }
            });
        
        config.NewConfig<(string AccessToken, RefreshToken RefreshToken), SignInResponse>()
            .ConstructUsing(src => new SignInResponse
            {
                AccessToken = src.AccessToken,
                RefreshToken = src.RefreshToken.Token,
                CurrentDate = src.RefreshToken.CreationDate.ToTimestamp()
            });
        
        config.NewConfig<(UserEntity user,  IEnumerable<PrivilegesDto> privileges), UserDto>()
            .ConstructUsing(src => new UserDto
            {
                Id = src.user.Id,
                Name = src.user.Name,
                Password = src.user.Password,
                Department = src.user.Department,
                Email = src.user.Email,
                Enabled = src.user.Enabled,
                AccessFailedCount = src.user.AccessFailedCount,
                GroupId = src.user.GroupId,
                RoleId = src.user.RoleId,
                PasswordLastChangedDate = src.user.LastPasswordChangedDate.ToTimestamp(),
                Properties = src.user.Properties,
                Description = src.user.Description,
                Privileges= { src.privileges },
                RegisterDate = src.user.RegisterDate.ToTimestamp(),
            });
    }
    
    private static void _AddSystemMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<(
                ListSystemsRequest request,
                IEnumerable<SystemDto> systems,
                int count), ListSystemsResponse>()
            .ConstructUsing(src => new ListSystemsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                Systems = { src.systems }
            });

        config.NewConfig<(string Id, string Title), SystemDto>()
            .ConstructUsing(src => new SystemDto
            {
                Id = src.Id,
                Title = src.Title
            });
    }
    
    private static void _AddFeatureMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<(ListFeaturesRequest request,
                IEnumerable<FeatureDto> features,
                int count), ListFeaturesResponse>()
            .ConstructUsing(src => new ListFeaturesResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                Features = { src.features }
            });
        
        config.NewConfig<(string Id, string Title), FeatureDto>()
            .ConstructUsing(src => new FeatureDto
            {
                Id = src.Id,
                Title = src.Title
            });
        
        config.NewConfig<(string Id, string Title, bool Enabled), FeatureDto>()
            .ConstructUsing(src => new FeatureDto
            {
                Id = src.Id,
                Title = src.Title,
                Enabled = src.Enabled
            });
    }
    
    private static void _AddPrivilegesMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<PrivilegesEntity, PrivilegesDto>();
        
        config.NewConfig<(
                ListPrivilegesRequest request,
                IEnumerable<PrivilegesDto> privileges,
                int count), ListPrivilegesResponse>()
            .ConstructUsing(src => new ListPrivilegesResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                Privileges = { src.privileges }
            });
        
        config.NewConfig<(string Id, string Title), PrivilegesDto>()
            .ConstructUsing(src => new PrivilegesDto
            {
                Id = src.Id,
                Title = src.Title,
            });
        
        config.NewConfig<(string Id, string Title, bool Enabled), PrivilegesDto>()
            .ConstructUsing(src => new PrivilegesDto
            {
                Id = src.Id,
                Title = src.Title,
                Enabled = src.Enabled
            });
    }

    private static void _AddRoleMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<(ListRolesRequest request, 
                IEnumerable<RoleDto> roles, 
                int count), ListRolesResponse>()
            .ConstructUsing(src => new ListRolesResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                Roles = { src.roles }
            });
        
        config.NewConfig<(string Id, string Title), RoleDto>()
            .ConstructUsing(src => new RoleDto
            {
                Id = src.Id,
                Title = src.Title
            });
        
        config.NewConfig<RoleDto, GetRoleResponse>()
            .ConstructUsing(src => new GetRoleResponse
            {
                Roles = { src }
            });
    }
    
    private static void _AddUserPrivilegeMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<(string Id, string PrivilegeId), UserPrivilegeEntity>()
            .ConstructUsing(src => new UserPrivilegeEntity
            {
                UserId = src.Id,
                PrivilegeId = src.PrivilegeId
            });
    }

    private static void _AddGroupFeatureMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<FeatureEntity, FeatureDto>();
        
        config.NewConfig<(string GroupId, string FeatureId), GroupFeatureEntity>()
            .ConstructUsing(src => new GroupFeatureEntity
            {
                GroupId = src.GroupId,
                FeatureId = src.FeatureId
            });
        
        config.NewConfig<IEnumerable<FeatureDto>, UpdateGroupFeaturesResponse>()
            .ConstructUsing(src => new UpdateGroupFeaturesResponse
            {
                Features = { src }
            });
        
        config.NewConfig<(
                GetGroupFeaturesRequest request,
                IEnumerable<FeatureDto> features,
                int count), GetGroupFeaturesResponse>()
            .ConstructUsing(src => new GetGroupFeaturesResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                Features = { src.features }
            });
    }
    
    private static void _AddGroupSystemMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<SystemEntity, SystemDto>();
        
        config.NewConfig<(string GroupId, string SystemId), GroupSystemEntity>()
            .ConstructUsing(src => new GroupSystemEntity
            {
                GroupId = src.GroupId,
                SystemId = src.SystemId
            });
        
        config.NewConfig<IEnumerable<SystemDto>, UpdateGroupSystemsResponse>()
            .ConstructUsing(src => new UpdateGroupSystemsResponse
            {
                Systems = { src }
            });
        
        config.NewConfig<(
                GetGroupSystemsRequest request,
                IEnumerable<SystemDto> systems,
                int count), GetGroupSystemsResponse>()
            .ConstructUsing(src => new GetGroupSystemsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                Systems = { src.systems }
            });
    }
}
