## Microservices with ASP.NET Core 5.0
### Overview
Implementation is based on the online course on Microservices. It aims to show the working and communication among separate stand-alone Microservices. Each microservice has its own database which includes MongoDB, Redis, PostgreSQL, MySQL and SQL Server. These microservices use event driven communication with RabbitMQ via MassTransit. API Gateway is based on Ocelot. Elasticsearch and Kibana is used for observability and distributed logging. Resilience is implemented with Polly. WatchDog is used to monitor the health check of all microservices. Portainer is setup to view the status of running Docker containers. OpenTelemetry and Jaeger is used for distributed tracing.

### TODO
Security will be implemented through IdentityServer4, JSON Web Token, OpenID Connect, Identity Model and OAuth 2.
Build pipeline will be setup using Azure Container Registry, Kubernetes, Azure Kubernetes Service and Azure DevOps.

## Installation & Setup
#### To start, run below command:
```
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d
```
#### To stop, run below command:
```
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml down
```

### Endpoints & Credentials
- Catalog Api = 8000
- Basket Api = 8001
- Discount Api = 8002
- Discount Grpc = 8003
- Ordering Api = 8004
- Shopping Aggregator = 8005
- Web Application = 8006
- Health Check Portal = 8007
- Ocelot Api Gateway = 8010
- Identify Server = 8015
- MongoDB Client = 3000
- pgAdmin4 = 5050 (admin@aspnetrun.com / Abcd1234)
- Kibana = 5601
- phpMyAdmin = 8080 (root / Abcd1234)
- Portainer = 9000 (admin / Abcd1234)
- Elasticsearch = 9200
- RabbitMQ = 15672 (guest / guest)
- Jaeger = 16686

### List of Packages
- AspNetCore.HealthChecks.MongoDb
- AspNetCore.HealthChecks.MySql
- AspNetCore.HealthChecks.NpgSql
- AspNetCore.HealthChecks.Rabbitmq
- AspNetCore.HealthChecks.Redis
- AspNetCore.HealthChecks.UI
- AspNetCore.HealthChecks.UI.Client
- AspNetCore.HealthChecks.UI.InMemory.Storage
- AspNetCore.HealthChecks.Uris
- AutoMapper
- AutoMapper.Extensions.Microsoft.DependencyInjection
- Dapper
- FluentValidation
- FluentValidation.DependencyInjectionExtensions
- Grpc.AspNetCore
- IdentityServer4
- IdentityServer4.EntityFramework
- MassTransit
- MassTransit.AspNetCore
- MassTransit.RabbitMQ
- MediatR.Extensions.Microsoft.DependencyInjection
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.Extensions.Caching.StackExchangeRedis
- Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
- Microsoft.Extensions.Http.Polly
- Microsoft.Extensions.Logging.Abstractions
- Microsoft.VisualStudio.Azure.Containers.Tools.Targets
- MongoDB.Driver
- Newtonsoft.Json
- Npgsql
- Ocelot
- Ocelot.Cache.CacheManager
- OpenTelemetry
- OpenTelemetry.Exporter.Console
- OpenTelemetry.Exporter.Jaeger
- OpenTelemetry.Extensions.Hosting
- OpenTelemetry.Instrumentation.AspNetCore
- OpenTelemetry.Instrumentation.Http
- Polly
- Pomelo.EntityFrameworkCore.MySql
- SendGrid
- Serilog.AspNetCore
- Serilog.Enrichers.Environment
- Serilog.Sinks.Elasticsearch
- Swashbuckle.AspNetCore
