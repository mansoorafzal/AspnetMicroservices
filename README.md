# Microservices
Implementation is based on the online course on Microservices. It aims to show the working and communication amoing separate stand-alone Microservices with RabbitMQ, MassTransit, gRPC, MediatR and Ocelot.

It includes following packages:
<br> AutoMapper
<br> AutoMapper.Extensions.Microsoft.DependencyInjection
<br> Dapper
<br> FluentValidation
<br> FluentValidation.DependencyInjectionExtensions
<br> Grpc.AspNetCore
<br> MassTransit
<br> MassTransit.AspNetCore
<br> MassTransit.RabbitMQ
<br> MediatR.Extensions.Microsoft.DependencyInjection
<br> Microsoft.EntityFrameworkCore.SqlServer
<br> Microsoft.EntityFrameworkCore.Tools
<br> Microsoft.Extensions.Caching.StackExchangeRedis
<br> Microsoft.Extensions.Logging.Abstractions
<br> Microsoft.VisualStudio.Azure.Containers.Tools.Targets
<br> MongoDB.Driver
<br> Newtonsoft.Json
<br> Npgsql
<br> Ocelot
<br> Ocelot.Cache.CacheManager
<br> SendGrid
<br> Swashbuckle.AspNetCore

Ports:
<br> Catalog Api = 5000, 8000
<br> Basket Api = 5001, 8001
<br> Discount Api = 5002, 8002
<br> Discount Grpc = 5003, 8003
<br> Ordering Api = 5004, 8004
<br> Shopping Aggregator = 5005, 8005
<br> Web Application = 5006, 8006
<br> Ocelot Api Gateway = 5010, 8010
