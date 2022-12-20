using GrandTripAPI.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GrandTripAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }

    public static partial class Extensions
    {
        #nullable enable
        public static int? GetId(this HttpContext context)
        {
            var header = context.Request.Headers["Authorization"].ToString().Split(" ");
            
            if (header.Length < 2) return null;

            var token = header[1];
            var service = context.RequestServices.GetRequiredService<JwtService>();
            
            return service.RetrieveId(token);
        }

        public static ILogger<T> L<T>(this HttpContext context)
            => context.RequestServices.GetRequiredService<ILogger<T>>();
#nullable disable
    }
}