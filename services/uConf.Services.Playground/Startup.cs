using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace uConf.Services.Playground
{
    public class Startup
    {
        private static IPNetwork _k8sNetwork = new IPNetwork(IPAddress.Parse("10.0.0.0"), 8);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                // Kubernetes cluster-local addresses
                options.KnownNetworks.Add(_k8sNetwork);
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Apply the "forwarded path base" header we configured in ambassador
            app.UseForwardedPathBase();
            app.UseForwardedHeaders();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapGet("/.headers", async (context) =>
                // {
                //     context.Response.ContentType = "text/plain";
                //     context.Response.StatusCode = 200;
                //     await context.Response.WriteAsync($"Host = {context.Request.Host}\n");
                //     await context.Response.WriteAsync($"Protocol = {context.Request.Protocol}\n");
                //     await context.Response.WriteAsync($"Path = {context.Request.Path}\n");
                //     await context.Response.WriteAsync($"PathBase = {context.Request.PathBase}\n");
                //     await context.Response.WriteAsync($"ClientIP = {context.Connection.RemoteIpAddress}\n");
                //     await context.Response.WriteAsync($"TraceId = {context.TraceIdentifier}\n");
                //     await context.Response.WriteAsync($"ActivityId = {Activity.Current.Id}\n");
                //     await context.Response.WriteAsync("---");
                //     foreach (var (name, value) in context.Request.Headers)
                //     {
                //         await context.Response.WriteAsync($"{name} = {value}\n");
                //     }
                // });
                endpoints.MapRazorPages();
            });
        }
    }
}
