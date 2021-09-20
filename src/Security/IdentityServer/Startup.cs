using HealthChecks.UI.Client;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace IdentityServer
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
            services.AddControllersWithViews();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var connectionString = Configuration.GetConnectionString("IdentityServerConnectionString");

            services.AddIdentityServer()
                .AddTestUsers(TestUsers.Users)
                .AddConfigurationStore<ConfigurationDbContext>(options =>
                {
                    options.ConfigureDbContext = sql => sql.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptionsAction: sqlOptions => 
                    {
                        sqlOptions.EnableRetryOnFailure();
                        sqlOptions.MigrationsAssembly(migrationsAssembly);
                    });
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = sql => sql.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure();
                        sqlOptions.MigrationsAssembly(migrationsAssembly);
                    });
                })
                .AddDeveloperSigningCredential();

            services
                .AddHealthChecks()
                .AddMySql(Configuration["ConnectionStrings:IdentityServerConnectionString"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
