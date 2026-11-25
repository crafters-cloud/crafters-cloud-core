# Backlog

Open work for `crafters-cloud-core`. Completed items are in [BACKLOG-DONE.md](BACKLOG-DONE.md).

**Conventions**

- Items are grouped by priority — **High** (next up: security, broken builds), **Medium** (soon), **Low** (nice to have, ideas) — and ordered by importance within each group.
- Each item has an ID (`CORE-nnn`, never reused), a title, the date it was added and a short description.
- When an item is done, move it to the top of `BACKLOG-DONE.md` and add `Completed: <date>` and a link to the PR.

## High

### CORE-002 Release stable 3.1.0

Added: 2026-11-22

Release `3.1.0` stable on .NET 10 with the dependency fixes only. Consumers of 3.1.0-preview.3 currently get vulnerable transitive packages, and stable-only consumers are still on 3.0.2 (.NET 9, out of support).

Status: `v3.1.0` is tagged and on MyGet, but not on NuGet: the push failed because the NuGet API key expired. To finish: create a new key on nuget.org, update the `NUGET_CRAFTERS_CLOUD_CORE_CI_API_KEY` secret, merge CORE-003, tag `v3.1.1`.

### CORE-003 A MyGet failure must not block the NuGet push

Added: 2026-11-22 · PR #22

In `ci.yml`, "Push to NuGet" is skipped when "Push to MyGet" fails, and `push.ps1` fails on versions already on the feed, so a release can't be re-run. Keep MyGet (it gets packages out faster), but let its step fail without blocking NuGet, and push with `--skip-duplicate`. Blocks CORE-002.

## Medium

### CORE-012 4.0.0: move to .NET 11

Added: 2026-11-22

Target `net11.0` only (no multi-targeting; .NET 10 users stay on 3.1.x). One upgrade covering .NET 11 / C# 15, EF Core 11, Aspire 17 and the other new major versions. Breaking change, hence 4.0.0.

Status: draft PR #23 (no code changes needed). Merge only after `v3.1.1` is tagged from `main` (CORE-002).

### CORE-013 Redesign the `Result` types with union types

Added: 2026-11-22

After CORE-012, rework the `Result` types with C# 15 unions. Decided 2026-11-24 (see also Milan Jovanović, "Can C# 15 Union Types Replace the Result Pattern?"):

1. Core ships `QueryResult<T>(T, NotFoundResult)`, `CreateResult<T>(CreatedResult<T>, BadRequestResult)`, `UpdateResult(NoContentResult, NotFoundResult, BadRequestResult)` and `CommandResult<T, TError>(T, TError, BadRequestResult)` for use cases with their own failure records. Apps can declare their own unions.
2. Case types keep their `*Result` names (no clash with ASP.NET `HttpResults`) and become sealed records. Drop `IResult`, `IValueResult<T>`, `IErrorResult<T>` and `SuccessResult<T>`; keep the `Result.*` factory.
3. `BadRequestResult` carries a core-owned `ValidationError(PropertyName, Message, Code?)`; FluentValidation leaves `Core` and stays in `Core.MediatR` and `AspNetCore`.
4. Remove OneOf completely. `ValidationBehavior` builds the `BadRequestResult` case through the union's constructor (`IUnion`).
5. `Core.AspNetCore` ships `ToMinimalApiResult` helpers for the three standard unions; endpoints `switch` over `CommandResult<T, TError>` themselves.
6. Caching: a JSON converter makes unions round-trip through the distributed cache. Failure cases are not cached by default; configuration (`DefaultFailureCases`, per entry `FailureCases`) can cache them per query type with their own expiration.

Status: implemented in draft PR #24 (stacked on #23). Breaking for consumers; the reference architecture migration is RA-016. Merge after #23, then retarget #24 to `main` so CI runs.

### CORE-005 Create GitHub Releases on `v*` tags

Added: 2026-11-22

Releases are only tags plus packages; the last GitHub Release is v2.1.0. Have CI create a GitHub Release (pre-release for `-preview` tags) with generated notes.

## Low

### CORE-014 Authorization on commands and queries

Added: 2026-11-22

Permissions are only checked at HTTP endpoints (`UserHasPermission`). A MediatR behavior with per-request authorization rules would also protect commands sent from background jobs or message handlers. Closed PR #1 has a starting point (`AuthorizationBehavior`, `IAuthorizationRule`).

### CORE-004 Protect the `main` branch

Added: 2026-11-22

`main` allows direct pushes and force-pushes. Postponed while there is a single contributor; when that changes, require a PR with passing CI and block force-pushes.

### CORE-009 Revisit NSwag vs built-in OpenAPI

Added: 2026-11-22

Keep NSwag (`Swagger`, `SmartEnums.Swagger`). Revisit if NSwag stops supporting a new .NET version or limits us; the alternative is built-in OpenAPI with schema transformers.

### CORE-011 Revisit the Verify 32.x pin

Added: 2026-11-22

Verify 33+ adds a build-time SponsorCheck that fails builds, and `Tests.Shared` would pass it on to consumers. Decided to stay on 32.x; revisit if 32.x stops getting fixes or a 33+ feature is needed.
