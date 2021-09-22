using AspnetRunBasics.Services;
using Common.Logging;
using Common.Policy;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;

namespace AspnetRunBasics
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
            services.AddTransient<LoggingDelegatingHandler>();

            services
                .AddHttpClient<ICatalogService, CatalogService>(c => c.BaseAddress = new Uri(Configuration["ApiSettings:GatewayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                .AddPolicyHandler(PollyPolicy.GetRetryPolicy())
                .AddPolicyHandler(PollyPolicy.GetCircuitBreakerPolicy());

            services
                .AddHttpClient<IBasketService, BasketService>(c => c.BaseAddress = new Uri(Configuration["ApiSettings:GatewayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                .AddPolicyHandler(PollyPolicy.GetRetryPolicy())
                .AddPolicyHandler(PollyPolicy.GetCircuitBreakerPolicy());

            services
                .AddHttpClient<IOrderService, OrderService>(c => c.BaseAddress = new Uri(Configuration["ApiSettings:GatewayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                .AddPolicyHandler(PollyPolicy.GetRetryPolicy())
                .AddPolicyHandler(PollyPolicy.GetCircuitBreakerPolicy());

            services.AddRazorPages();

            services
                .AddHealthChecks()
                .AddUrlGroup(new Uri(Configuration["ApiSettings:GatewayAddress"]), "Ocelot API Gateway", HealthStatus.Degraded);

            services.AddOpenTelemetryTracing((builder) =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("AspnetRunBasics"))
                    .AddHttpClientInstrumentation()
                    .AddSource(nameof(CatalogService))
                    .AddSource(nameof(BasketService))
                    .AddSource(nameof(OrderService))
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = "localhost";
                        options.AgentPort = 6831;
                        options.ExportProcessorType = ExportProcessorType.Simple;
                    })
                    .AddConsoleExporter(options =>
                    {
                        options.Targets = ConsoleExporterOutputTargets.Console;
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
