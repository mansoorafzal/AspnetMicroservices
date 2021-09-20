using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Polly;
using System;
using System.Linq;

namespace IdentityServer.Data
{
    public class QuickStartContextSeed
    {
        public static void SeedAsync(ConfigurationDbContext context, ILogger<QuickStartContextSeed> logger)
        {
            try
            {
                logger.LogInformation("Migrating mysql database.");

                var retry = Policy.Handle<MySqlException>()
                            .WaitAndRetry(
                                retryCount: 5,
                                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                onRetry: (exception, retryCount, context) =>
                                {
                                    logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                                });

                retry.Execute(() => ExecuteMigrations(context));

                logger.LogInformation("Migrated mysql database.");
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the mysql database");
            }
        }

        private static void ExecuteMigrations(ConfigurationDbContext context)
        {
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
    }
}
