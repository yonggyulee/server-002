using Mapster;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Update.Protos.V1;
using RcSetupVersionEntity = Mirero.DAQ.Domain.Update.Entity.RcSetupVersion;
using RcSetupVersionDto = Mirero.DAQ.Domain.Update.Protos.V1.RcSetupVersion;
using MppSetupVersionEntity = Mirero.DAQ.Domain.Update.Entity.MppSetupVersion;
using MppSetupVersionDto = Mirero.DAQ.Domain.Update.Protos.V1.MppSetupVersion;

namespace Mirero.DAQ.Service.Extensions.Update;

public static class MapperExtension
{
    public static TypeAdapterConfig AddUpdateMapperConfig(this TypeAdapterConfig config)
    {
        config.NewConfig<(ListMppSetupVersionsRequest request,
                List<MppSetupVersionDto> mpps,
                int count), ListMppSetupVersionsResponse>()
            .ConstructUsing(src => new ListMppSetupVersionsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                MppSetupVersions = { src.mpps }
            });

        config.NewConfig<(ListRcSetupVersionsRequest request,
                List<RcSetupVersionDto> rcs,
                int count), ListRcSetupVersionsResponse>()
            .ConstructUsing(src => new ListRcSetupVersionsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.request.QueryParameter.PageIndex,
                    PageSize = src.request.QueryParameter.PageSize,
                    Count = src.count
                },
                RcSetupVersions = { src.rcs }
            });

        return config;
    }
}