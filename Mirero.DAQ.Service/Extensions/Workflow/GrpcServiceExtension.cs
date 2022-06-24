using Microsoft.AspNetCore.Builder;
using Mirero.DAQ.Service.Services.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Service.Extensions.Workflow;

public static class GrpcServiceExtension
{
    public static void UseWorkflowService(this WebApplication app)
    {
        app.MapGrpcService<GrpcVolumeService>();
        app.MapGrpcService<GrpcServerService>();
        app.MapGrpcService<GrpcWorkerService>();
        app.MapGrpcService<GrpcWorkflowService>();
        app.MapGrpcService<GrpcJobService>();
    }
}
