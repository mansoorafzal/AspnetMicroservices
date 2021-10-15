using Common.Infrastructure;
using EventBus.Messages.Common;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Ordering.API.Controllers;
using Ordering.API.EventBusConsumer;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using RabbitMQ.Client;
using System;

namespace Ordering.API
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
            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);

            services.AddMassTransit(config => {
                config.AddConsumer<BasketCheckoutConsumer>();
                config.UsingRabbitMq((context, configuration) => {
                    configuration.Host(Configuration["EventBusSettings:HostAddress"]);
                    configuration.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c => {
                        c.ConfigureConsumer<BasketCheckoutConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<BasketCheckoutConsumer>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
            });

            services
                .AddHealthChecks()
                .AddDbContextCheck<OrderContext>()
                .AddRabbitMQ(_ =>
                {
                    var factory = new ConnectionFactory()
                    {
                        Uri = new Uri(Configuration["EventBusSettings:HostAddress"]),
                        AutomaticRecoveryEnabled = true
                    };

                    return factory.CreateConnection();
                });

            services.AddOpenTelemetryTracing((builder) =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Ordering.API"))
                    .AddHttpClientInstrumentation()
                    .AddSource(nameof(OrderController))
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

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["IdentityServer:Uri"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim("client_id", "shop_mvc_client"));
            });

            services.AddConsulServices(Configuration.GetServiceConfig());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
