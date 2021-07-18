using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace IdentityServer.Data
{
    public class QuickStartContextSeed
    {
        public static void SeedAsync(ConfigurationDbContext context, ILogger<QuickStartContextSeed> logger, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                logger.LogInformation("Migrating mysql database.");

                context.Database.Migrate();

                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }

                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }

                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the mysql database");

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;

                    System.Threading.Thread.Sleep(2000);
                    SeedAsync(context, logger, retryForAvailability);
                }
            }
        }
    }
}
