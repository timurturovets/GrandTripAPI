using System.Threading.Tasks;
using GrandTripAPI.Data.Repositories;
using GrandTripAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GrandTripAPI
{
    public static class Extensions
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

        public static async Task<User?> GetUser(this HttpContext context)
        {
            var id = context.GetId();
            var repo = context.RequestServices.GetRequiredService<UserRepository>();
            var user = await repo.GetBy(u => u.Id == id);
            return user;
        }
        public static ILogger<T> L<T>(this HttpContext context)
            => context.RequestServices.GetRequiredService<ILogger<T>>();
        
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthMiddleware>();
            return app;
        }
#nullable disable
    }
}