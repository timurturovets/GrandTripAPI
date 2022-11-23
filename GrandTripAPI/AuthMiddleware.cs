using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GrandTripAPI
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var token = context
                    .Request
                    .Headers["Authorization"]
                    .ToString()
                    .Split(" ")
                    [1];

                if (!string.IsNullOrEmpty(token)) context.Session.SetString("token", token);
            }
            catch (IndexOutOfRangeException) { }

            await _next(context);
        }
    }

    public static partial class Extensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthMiddleware>();
            return app;
        }
    }
}