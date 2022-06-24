using System.Net;
using Mapster;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Inference.Protos.V1;

using ServerEntity = Mirero.DAQ.Domain.Inference.Entities.Server;
using VolumeEntity = Mirero.DAQ.Domain.Inference.Entities.Volume;

namespace Mirero.DAQ.Service.Extensions.Inference;

public static class InferenceMapperExtension
{
    public static TypeAdapterConfig AddInferenceMapperConfig(this TypeAdapterConfig config)
    {
        _AddVolumeMapperConfig(config);

        _AddServerMapperConfig(config);

        _AddWorkerMapperConfig(config);

        _AddModelMapperConfig(config);

        _AddModelVersionMapperConfig(config);

        return config;
    }

    private static void _AddVolumeMapperConfig(TypeAdapterConfig config)
    {
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
                Volumes = { src.Volumes }
            });

        // Update 시 Usage 수정 방지.
        config.NewConfig<VolumeEntity, VolumeEntity>()
            .Ignore(v => v.Usage);
    }

    private static void _AddServerMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<(ListServersRequest Request
                , IEnumerable<Server> Servers
                , int Count),
                ListServersResponse>()
            .ConstructUsing(src => new ListServersResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                Servers = { src.Servers }
            });

        //config
        //    .NewConfig<Server, ServerEntity>()
        //    .Map(dest => dest.Address, src => IPAddress.Parse(src.Address));

        config
            .NewConfig<ServerEntity, Server>()
            .Map(dest => dest.Address, src => src.Address.ToString());
    }

    private static void _AddWorkerMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<(ListWorkersRequest Request
                , IEnumerable<Worker> InferenceWorkers
                , int Count),
                ListWorkersResponse>()
            .ConstructUsing(src => new ListWorkersResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                Workers = { src.InferenceWorkers }
            });
    }

    private static void _AddModelMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<(ListModelsRequest Request
                , IEnumerable<Model> Models
                , int Count),
                ListModelsResponse>()
            .ConstructUsing(src => new ListModelsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                Models = { src.Models }
            });
    }

    private static void _AddModelVersionMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<(ListModelVersionsRequest Request
                , IEnumerable<ModelVersion> ModelVersions
                , int Count),
                ListModelVersionsResponse>()
            .ConstructUsing(src => new ListModelVersionsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                ModelVersions = { src.ModelVersions }
            });

        //config.NewConfig<ModelVersion, ModelVersionEntity>()
        //    .Map(dest => dest.Buffer, 
        //        src => src.Buffer.ToByteArray());

        //config.NewConfig<ModelVersionEntity, ModelVersion>()
        //    .Map(dest => dest.Buffer, 
        //        src => src.Buffer == null ? null : ByteString.CopyFrom(src.Buffer));
    }
}