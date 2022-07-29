using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace vbSparkle.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)

                .UseKestrel()
                .CaptureStartupErrors(true)
                .ConfigureKestrel((context, options) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        options.Listen(IPAddress.Any, 80);
                    }
                })
                .UseStartup<Startup>();
        }
    }
}
