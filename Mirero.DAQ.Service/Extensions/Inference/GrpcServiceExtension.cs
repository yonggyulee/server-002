using Microsoft.AspNetCore.Builder;
using Mirero.DAQ.Service.Services.Inference;

namespace Mirero.DAQ.Service.Extensions.Inference;

public static class GrpcServiceExtension
{
    public static void UseInferenceService(this WebApplication app)
    {
        app.MapGrpcService<GrpcVolumeService>();
        app.MapGrpcService<GrpcServerService>();
        app.MapGrpcService<GrpcWorkerService>();
        app.MapGrpcService<GrpcModelService>();
        app.MapGrpcService<GrpcInferenceService>();
    }
}