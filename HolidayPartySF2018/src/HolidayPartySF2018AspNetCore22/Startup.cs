using System.Linq;
using HolidayPartySF2018AspNetCore22.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HolidayPartySF2018AspNetCore22
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
            // Registers required services for health checks
            services.AddHealthChecks()
                .AddGCInfoCheck("GCInfo", thresholdInBytes: 1024L * 1024L * 1L);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            // health check
            app.Use((context, next) => context.Request.Path.StartsWithSegments("/hc")
                ? context.Response.WriteAsync(string.Empty)
                : next()
            );

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                // This custom writer formats the detailed status as JSON.
                ResponseWriter = (httpContext, result) =>
                {
                    httpContext.Response.ContentType = "application/json";

                    var json = new JObject(
                        new JProperty("status", result.Status.ToString()),
                        new JProperty("results", new JObject(result.Entries.Select(pair =>
                            new JProperty(pair.Key, new JObject(
                                new JProperty("status", pair.Value.Status.ToString()),
                                new JProperty("description", pair.Value.Description),
                                new JProperty("data", new JObject(pair.Value.Data.Select(p => new JProperty(p.Key, p.Value))))))))));
                    return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
                }
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}