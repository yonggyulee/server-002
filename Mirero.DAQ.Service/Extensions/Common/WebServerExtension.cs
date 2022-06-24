using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Mirero.DAQ.Service.Extensions.Common;

public static class WebServerExtension
{
    public static IWebHostBuilder AddKestrelServer(this IWebHostBuilder builder, IConfiguration configuration)
    {
        builder.ConfigureKestrel(options =>
        {
            // options.Listen(IPAddress.Any, 5001, listenOptions =>
            // {
            //     listenOptions.Protocols = HttpProtocols.Http2;
            //     listenOptions.UseConnectionLogging();
            // });
        });
        
        return builder;
    }
}