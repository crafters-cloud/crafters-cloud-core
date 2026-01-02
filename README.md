# Crafters Cloud Core

![CI](https://github.com/crafters-cloud/crafters-cloud-core/workflows/CI/badge.svg)
[![NuGet](https://img.shields.io/nuget/dt/CraftersCloud.Core.svg)](https://www.nuget.org/packages/CraftersCloud.Core)
[![NuGet](https://img.shields.io/nuget/vpre/CraftersCloud.Core.svg)](https://www.nuget.org/packages/CraftersCloud.Core)
[![MyGet (dev)](https://img.shields.io/myget/crafters-cloud/v/CraftersCloud.Core.svg)](https://myget.org/gallery/crafters-cloud)

CraftersCloud Core is a set of .NET libraries that provides infrastructure and utilities for building applications with
Minimal Api, Entity Framework and MediatR.

It includes features such as domain event handling, transaction management, smart enums, swagger, repository
implementations, health-checks, authentication/authorization, unit testing and more.

## Features

### AspNetCore

- Minimal Api
- Authorization
- Error handling
- Validation

### Core

- Domain events
- Entities, Repositories, Unit of work
- Paging
- Command/Queries (CQRS)
- Entities
- Results
- StronglyTypedIds
- Various utilities and extensions

### Core.Mediatr

- Mediatr integration (e.g. Validation, Logging)

### Core.SourceGenerator

- Source generators for the Core library (e.g. StronglyTypedIds)

### EntityFramework

- Seeding
- IQueryable extensions

### EntityFramework.Infrastructure

- MediatR pipeline for the transaction management
- Unit of work implementation
- Entity framework repository implementation
- MediatR pipeline for the publishing of the domain events

### EventBus

    - TBD

### HealthChecks

    - HealthChecks Authorization
    - HealthChecks registration extensions

### Infrastructure

    - Common infrastructure utilities
    - Default TimeProvider implementation

### IntegrationEvents

    - Support for the integration events

### SmartEnums

    - Provides support for Smart enums (based on Ardalis.SmartEnum library)

### SmartEnums.EntityFramework

    - Provides support for persisting smart enums with Entity Framework

### SmartEnums.Swagger

    - Provides support for generating Swagger documentation for smart enums

### SmartEnums.SystemTextJson

    - Provides support for (de)serialization of SmartEnums

### Swagger

    - NSwag generation utilities

### Tests.Shared

    - Shared classes for tests

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- Visual Studio or JetBrains Rider

### Installation

To install the library, add any of the following packages starting with "CraftersCloud.Core" to your project:

```sh
dotnet add package CraftersCloud.Core.AspNetCore
dotnet add package CraftersCloud.Core.MediatR
dotnet add package CraftersCloud.Core.SourceGenerator
dotnet add package CraftersCloud.Core
dotnet add package CraftersCloud.Core.EntityFramework
dotnet add package CraftersCloud.Core.Core.EntityFramework
dotnet add package CraftersCloud.Core.EventBus
dotnet add package CraftersCloud.Core.HealthChecks
dotnet add package CraftersCloud.Core.Infrastructure
dotnet add package CraftersCloud.Core.IntegrationEvents
dotnet add package CraftersCloud.Core.SmartEnums.EntityFramework
dotnet add package CraftersCloud.Core.SmartEnums.Swagger
dotnet add package CraftersCloud.Core.SmartEnums.SystemTextJson
dotnet add package CraftersCloud.Core.SmartEnums
dotnet add package CraftersCloud.Core.Swagger
dotnet add package CraftersCloud.Core.Tests.Shared
```