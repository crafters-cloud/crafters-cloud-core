# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A set of .NET 10 libraries (published as `CraftersCloud.Core.*` NuGet packages) providing infrastructure for apps built on Minimal API + EF Core + MediatR. There is no runnable app here — only `src/` libraries and `tests/`.

## Commands

```powershell
dotnet build                                   # build everything (CraftersCloud.Core.slnx; SDK pinned via global.json)
dotnet test                                    # run all tests
dotnet test tests/Core.Tests                   # one test project
dotnet test --filter "FullyQualifiedName~CollectionUpdaterExtensionsFixture"   # one fixture
dotnet test --filter "Name=GetValue"           # one test method
./build.ps1                                    # what CI runs: clean, Release build, test, pack IsPackable projects into ./artifacts
```

CI (`.github/workflows/ci.yml`) runs `build.ps1` on every push; pushes to `release` or `v*` tags also publish to MyGet and NuGet via `push.ps1`.

## Build rules that will bite you

- `Directory.Build.props` sets `TreatWarningsAsErrors`, `EnforceCodeStyleInBuild`, `AnalysisLevel 10.0` and `Features=strict` — analyzer/style warnings (incl. `.editorconfig` rules) fail the build.
- Central package management: versions live in `Directory.Packages.props`; `<PackageReference>` in csproj files must not carry a `Version`.
- `RestorePackagesWithLockFile` is on — every project has a `packages.lock.json` that must be updated (by restoring) when dependencies change.
- Versioning is via MinVer from git tags with prefix `v` (no version in project files).
- Only projects with `<IsPackable>true</IsPackable>` get packed.
- NuGet audit runs on restore, so a vulnerable transitive package fails the build (NU1902/NU1903) — fix by bumping the package that pulls it in.
- Deliberately pinned versions (also encoded as Dependabot ignores in `.github/dependabot.yml`): **MediatR 12.5.0** (13+ is commercially licensed), **Verify 32.x** (33+ adds a buildTransitive SponsorCheck that fails builds — and `Tests.Shared` would propagate it to consumers), **Microsoft.CodeAnalysis.CSharp 5.0.0** (a source generator can't reference a newer Roslyn than the consumer's SDK ships).
- SourceLink comes from the .NET SDK itself; don't add `Microsoft.SourceLink.GitHub`.
- `JetBrains.Annotations` is globally imported; public API types are typically marked `[PublicAPI]`, DI/reflection-activated types `[UsedImplicitly]`.

## Tests

Test projects import `Tests.Build.props`: NUnit + Shouldly (both globally imported) + FakeItEasy. Test classes are named `*Fixture` and tagged with `[Category("unit")]`. `src/Tests.Shared` (and `src/AspireTests.Shared`) are packaged helpers for *consumers'* tests, not test projects themselves.

## Architecture

Project dependency layering (namespaces are `CraftersCloud.Core.<ProjectName>`):

- **Core** — the base everything references: entities (`Entity` with domain events, `EntityWithTypedId`), `IRepository`/`IUnitOfWork` abstractions, CQRS marker interfaces (`ICommand`, `IQuery`, handlers) on top of MediatR, `Result` types (Success/Created/NotFound/Forbidden/Error/NoContent/BadRequest), paging, `IStronglyTypedId<T>`.
- **Core.MediatR** — pipeline behaviors (validation, logging, caching via `Core.Caching*`).
- **EntityFramework** (EF utilities, seeding) → **EntityFramework.Infrastructure** (`BaseDbContext`, EF repository, `DbContextUnitOfWork`, Autofac registration in `ContainerBuilderExtensions`).
- **AspNetCore** (Minimal API endpoint discovery via `IEndpoint` + `AddCoreEndpoints`/`MapCoreEndpoints`, permission-based authorization, exception handling, FluentValidation filter), **Swagger** (NSwag), **HealthChecks**, **Infrastructure**.
- **SmartEnums** (Ardalis.SmartEnum) with sibling adapters `.EntityFramework`, `.Swagger`, `.SystemTextJson`. **Core.SystemTextJson** is the equivalent adapter for strongly-typed IDs.
- **IntegrationEvents** → **EventBus** (Azure Service Bus).

### Command pipeline (cross-cutting behavior spread across projects)

1. Commands implement `ICommand`/`ICommand<T>`, which expose `TransactionBehavior` (default: ReadCommitted transaction; override with `CommandTransactionBehavior.NoTransaction` or `TransactionWith(level)`).
2. `SaveChangesBehavior<TDbContext,...>` (EntityFramework.Infrastructure, abstract — consumers subclass it per DbContext) wraps the handler: skips queries and nested calls when a transaction is already active, otherwise opens a transaction inside EF's execution strategy, runs the handler, calls `IUnitOfWork.SaveChangesAsync`, and commits. Handlers therefore do not call SaveChanges themselves.
3. `PublishDomainEventsInterceptor` collects domain events from tracked entities *before* save (so deleted entities' events aren't lost) and publishes them via MediatR *after* save.

### Strongly-typed IDs source generator

`Core.SourceGenerator` targets `netstandard2.0` (Roslyn requirement) and is an incremental generator. It emits an internal `[StronglyTypedId(ValueKind.Guid|Int)]` attribute into the consuming assembly and generates the body for `partial record` types annotated with it. Tests reference it as `OutputItemType="Analyzer" ReferenceOutputAssembly="false"` (see `tests/Core.SourceGenerator.Tests`) — use the same pattern for any project that needs generated IDs.
