using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using RecommenderService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecommenderService.Domain.Repositories;
using RecommenderService.Infrastructure.Database.Repositiories;

namespace RecommenderService.gRPC
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddScoped<IRecommenderServiceRepository, SqlRecommenderServiceRepository>();
            services.AddDbContext<RecommenderServiceRepositoryDbContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("RecommenderServiceRepositoryDbContext"), sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(15), errorNumbersToAdd: null);
                });
            });

            services.AddScoped(typeof(SpotifyAPI.SpotifyRESTApi));
            services.AddScoped(typeof(SpotifyAPI.Auth.IdentityClient));
            services.AddScoped(typeof(SpotifyAPI.Auth.AccessTokenProvider));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<RecommenderServiceRepositoryDbContext>();
                context.Database.Migrate();
            }
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<SpotifyRecommenderService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
