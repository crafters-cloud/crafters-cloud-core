# CraftersCloud Core

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
dotnet add package CraftersCloud.Core
```

or

```sh
dotnet add package CraftersCloud.AspNetCore
```