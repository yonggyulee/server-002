using Google.Protobuf.WellKnownTypes;
using Mapster;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using FloorPlan = Mirero.DAQ.Domain.Gds.Entities.FloorPlan;
using VolumeEntity = Mirero.DAQ.Domain.Gds.Entities.Volume;
using ServerEntity = Mirero.DAQ.Domain.Gds.Entities.Server;
using GdsEntity = Mirero.DAQ.Domain.Gds.Entities.Gds;

namespace Mirero.DAQ.Service.Extensions.Gds;

public static class GdsMapperExtension
{
    public static void AddGdsServiceMapperConfig(this TypeAdapterConfig config)
    {
        AddVolumeMapperConfig(config);
        AddServerMapperConfig(config);
        AddGdsMapperConfig(config);
        AddFloorPlanMapperConfig(config);
    }

    private static void AddVolumeMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<VolumeEntity, Volume>();

        config
            .NewConfig<CreateVolumeRequest, VolumeEntity>();

        config
            .NewConfig<UpdateVolumeRequest, VolumeEntity>();

        config
            .NewConfig<(ListVolumesRequest Request
                , IEnumerable<Volume> Volumes
                , int Count),
                ListVolumesResponse>()
            .ConstructUsing(src => new ListVolumesResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                Volumes = {src.Volumes}
            });
    }

    private static void AddServerMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<ServerEntity, Server>()
            .Map(dest => dest.Address, src => src.Address.ToString());

        config
            .NewConfig<CreateServerRequest, ServerEntity>();

        config
            .NewConfig<UpdateServerRequest, ServerEntity>();

        config
            .NewConfig<(ListServersRequest Request
                , IEnumerable<Server> Servers
                , int Count),
                ListServersResponse>()
            .ConstructUsing(src => new ListServersResponse()
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                Servers = {src.Servers}
            });
    }

    private static void AddGdsMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<CreateGdsRequest, GdsEntity>();

        config
            .NewConfig<(ListGdsesRequest Request
                , IEnumerable<Domain.Gds.Protos.V1.Gds> Gds
                , int Count),
                ListGdsesResponse>()
            .ConstructUsing(src => new ListGdsesResponse()
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                Gdses = {src.Gds}
            });
    }

    private static void AddFloorPlanMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<FloorPlan, Domain.Gds.Protos.V1.FloorPlan>()
            .Map(dest => dest.RegisterDate, src => src.RegisterDate.ToTimestamp())
            .Map(dest => dest.UpdateDate, src => src.RegisterDate.ToTimestamp())
            .AfterMapping((src, dest) =>
            {
                var items = src
                    .FloorPlanGdses
                    .Select(x => x.Adapt<Domain.Gds.Protos.V1.FloorPlanGds>());
                dest.FloorPlanGdses.AddRange(items);
            });
    }
}