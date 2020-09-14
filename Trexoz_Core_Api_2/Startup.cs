using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Trexoz_Core_Api_2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddIdentityServer()
            .AddInMemoryClients(Clients.Get())
            .AddInMemoryIdentityResources(Core.Resources.GetIdentityResources())
            .AddInMemoryApiResources(Core.Resources.GetApiResources())
            .AddInMemoryApiScopes(Core.Resources.GetApiScopes())
            .AddTestUsers(Users.Get())
            .AddDeveloperSigningCredential();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Trexoz API", Version = "v1" });
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication("Bearer", options =>
            {
                options.ApiName = "api1";
                options.Authority = "https://localhost:44372/";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trexoz API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
