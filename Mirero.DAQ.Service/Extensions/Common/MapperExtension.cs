using Google.Protobuf.WellKnownTypes;
using Mapster;

namespace Mirero.DAQ.Service.Extensions.Common;

public static class MapperExtension
{
    public static TypeAdapterConfig AddCommonMapperConfig(this TypeAdapterConfig config)
    {
        config.NewConfig<DateTime, Timestamp>()
            .MapWith(d => DateTime.SpecifyKind(d, DateTimeKind.Utc).ToTimestamp());
        config.NewConfig<Timestamp?, DateTime>()
            .MapWith(t => t != null ?
                DateTime.SpecifyKind(t.ToDateTime(), DateTimeKind.Utc) 
                : DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
       
        return config;
    }
}