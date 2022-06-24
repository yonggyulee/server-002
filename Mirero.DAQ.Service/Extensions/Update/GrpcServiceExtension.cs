using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Mirero.DAQ.Service.Services.Inference;
using Mirero.DAQ.Service.Services.Update;

namespace Mirero.DAQ.Service.Extensions.Update;

public static class GrpcServiceExtension
{
    public static void UseUpdateService(this WebApplication app,  IConfiguration configuration)
    {
        app.MapGrpcService<GrpcMppUpdateService>();
        app.MapGrpcService<GrpcRcUpdateService>();
        _UseDirectoryBrowser(app, configuration);
    }
    
    private static void _UseDirectoryBrowser(this WebApplication app,  IConfiguration configuration)
    {
        var mppFileProvider = new PhysicalFileProvider(Path.Combine(configuration.GetValue<string>("Directory:MppSetupFileDirectory")));
        var rcFileProvider = new PhysicalFileProvider(Path.Combine(configuration.GetValue<string>("Directory:RcSetupFileDirectory")));
        var mppRequestPath = configuration.GetValue<string>("Directory:MppEndpoint");
        var rcRuestPath = configuration.GetValue<string>("Directory:RcEndpoint");
        
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = mppFileProvider,
            RequestPath = mppRequestPath
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = rcFileProvider,
            RequestPath = rcRuestPath
        });
        
        app.UseDirectoryBrowser(new DirectoryBrowserOptions
        {
            FileProvider = mppFileProvider,
            RequestPath = mppRequestPath
        });
    
        app.UseDirectoryBrowser(new DirectoryBrowserOptions
        {
            FileProvider = rcFileProvider,
            RequestPath = rcRuestPath
        });
    }
}