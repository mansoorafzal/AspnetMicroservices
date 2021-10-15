using Microsoft.Extensions.Configuration;
using System;

namespace Common.Infrastructure
{
    public static class ServiceConfigExtensions
    {
        public static ServiceConfig GetServiceConfig(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var serviceConfig = new ServiceConfig
            {
                ServiceId = configuration.GetValue<string>("ServiceConfig:ServiceId"),
                ServiceName = configuration.GetValue<string>("ServiceConfig:ServiceName"),
                ServiceAddress = configuration.GetValue<Uri>("ServiceConfig:ServiceAddress"),
                ServiceDiscoveryAddress = configuration.GetValue<Uri>("ServiceConfig:ServiceDiscoveryAddress")
            };

            return serviceConfig;
        }
    }
}
