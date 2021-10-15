using System;

namespace Common.Infrastructure
{
    public class ServiceConfig
    {
        public string ServiceId { get; set; }

        public string ServiceName { get; set; }

        public Uri ServiceAddress { get; set; }

        public Uri ServiceDiscoveryAddress { get; set; }
    }
}
