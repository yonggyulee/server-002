using Microsoft.AspNetCore.Builder;
using Mirero.DAQ.Service.Services.Dataset;

namespace Mirero.DAQ.Service.Extensions.Dataset;

public static class GrpcServiceExtension
{
    public static void UseDatasetService(this WebApplication app)
    {
        app.MapGrpcService<GrpcVolumeService>();
        app.MapGrpcService<GrpcClassCodeService>();
        app.MapGrpcService<GrpcGtDatasetService>();
        app.MapGrpcService<GrpcImageDatasetService>();
        app.MapGrpcService<GrpcTrainingDataService>();
    }
}