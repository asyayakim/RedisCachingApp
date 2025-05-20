# Redis Caching Demo Application 

[![.NET](https://img.shields.io/badge/.NET-9.0-%23512BD4)](https://dotnet.microsoft.com/)
[![Redis](https://img.shields.io/badge/Redis-%23DC382D?logo=redis&logoColor=white)](https://redis.io/)

A practical demonstration of Redis distributed caching in .NET using `IDistributedCache` and `Microsoft.Extensions.Caching.StackExchangeRedis`.

## Key Features 

-  **Redis Configuration**  
  Easy setup with local Redis instance (default: `localhost:6379`)
-  **Expiration Policies**  
  Combined absolute (10min) and sliding expiration (2min)
-  **Cache Monitoring**  
  Detailed console logging for cache operations:
  - Cache hits/misses
  - Entry additions
  - Manual invalidations
  - Expiration triggers
-  **Interactive Demo**  
  Simulated cache access patterns with manual control

## Getting Started 

### Prerequisites
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download) or later
- Local [Redis Server](https://redis.io/download) (Docker recommended)
- Required NuGet Packages:
  ```bash
  Microsoft.Extensions.Caching.StackExchangeRedis (7.0.0+)
  Microsoft.Extensions.DependencyInjection (7.0.0+)
