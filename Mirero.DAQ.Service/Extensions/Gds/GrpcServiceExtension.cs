using Microsoft.AspNetCore.Builder;
using Mirero.DAQ.Service.Services.Gds;

namespace Mirero.DAQ.Service.Extensions.Gds
{
    public static class GrpcServiceExtension
    {
        public static void UseGdsService(this WebApplication app)
        {
            app.MapGrpcService<GrpcVolumeService>();
            app.MapGrpcService<GrpcServerService>();
            app.MapGrpcService<GrpcGdsService>();
        }
    }
}
