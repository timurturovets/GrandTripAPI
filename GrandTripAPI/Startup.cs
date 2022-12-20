using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using GrandTripAPI.Data;
using GrandTripAPI.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
namespace GrandTripAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            }));
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options
                => options.UseSqlServer(connection, 
                    o=>
                        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

            services.AddSession();
            services.AddDistributedMemoryCache();
            
            services.AddTransient<RouteRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<JwtService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSession();
            app.UseRouting();
            app.UseCors(o=>o.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            
            app.UseAuthMiddleware();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}