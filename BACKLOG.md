# Backlog

Open work for `crafters-cloud-core`. Completed items are in [BACKLOG-DONE.md](BACKLOG-DONE.md).

**Conventions**

- Items are grouped by priority — **High** (next up: security, broken builds), **Medium** (soon), **Low** (nice to have, ideas) — and ordered by importance within each group.
- Each item has an ID (`CORE-nnn`, never reused), a title, the date it was added and a short description.
- When an item is done, move it to the top of `BACKLOG-DONE.md` and add `Completed: <date>` and a link to the PR.

## High

### CORE-001 Fix vulnerable dependencies — `main` no longer builds

Added: 2026-11-22

NuGet audit fails restore on vulnerabilities published since 3.1.0-preview.3: `Aspire.Hosting` / `Aspire.Hosting.AppHost` 13.6.0 (high), `MessagePack` 2.5.302 (moderate, via Aspire), `System.Security.Cryptography.Pkcs` 9.0.11 (high). Dependabot PRs #16–#19 overlap with this: #17 and #18 pass CI, #16 (microsoft group) fails, #19 is Aspire 17 (major version, fails).

### CORE-002 Release a patched version

Added: 2026-11-22

After CORE-001, release `3.1.0-preview.4` or `3.1.0` stable. Consumers of 3.1.0-preview.3 currently get vulnerable transitive packages (e.g. `System.Security.Cryptography.Pkcs` 10.0.10 via EF Core 10.0.12).

## Medium

### CORE-003 A MyGet failure must not block the NuGet push

Added: 2026-11-22

In `ci.yml`, "Push to NuGet" is skipped when "Push to MyGet" fails. On 2026-10-03 MyGet returned 403 and nothing was published until the API key was fixed.

### CORE-004 Protect the `main` branch

Added: 2026-11-22

`main` allows force-pushes and direct pushes. Require a PR with passing CI and block force-pushes.

### CORE-005 Create GitHub Releases on `v*` tags

Added: 2026-11-22

Releases are only tags plus packages; the last GitHub Release is v2.1.0. Have CI create a GitHub Release (pre-release for `-preview` tags) with generated notes.

### CORE-006 Triage old pull requests and branches

Added: 2026-11-22

PRs #1–#3 (from 2024-11) are open with no activity: authorization behavior, database extensions, queryable extensions. Merge or close them and delete their branches. `features/3.0` is merged and can be deleted.

## Low

### CORE-007 Remove the FluentValidation dependency from Core

Added: 2026-11-22

`Core` references FluentValidation only so `BadRequestResult` can carry `ValidationFailure`. Breaking change; do in a major version.

### CORE-008 Reconsider the Autofac dependency

Added: 2026-11-22

Used for repository registration (`EntityFramework.Infrastructure`) and `EventBus`. The built-in DI container plus assembly scanning could replace it and drop a dependency for consumers.

### CORE-009 Evaluate built-in OpenAPI instead of NSwag

Added: 2026-11-22

`Swagger` and `SmartEnums.Swagger` use NSwag. .NET 10 has built-in OpenAPI 3.1 document generation with schema transformers.

### CORE-010 Plan the next target framework

Added: 2026-11-22

Stay on .NET 10 (LTS, supported until 2028-11-14). .NET 11 is STS with the same end of support, so no multi-targeting unless a consumer needs it. Move to .NET 12 (LTS, expected 2027-11).

### CORE-011 Revisit the Verify 32.x pin

Added: 2026-11-22

Verify 33+ adds a build-time SponsorCheck that fails builds, and `Tests.Shared` would pass it on to consumers. Decide between sponsoring/declaring an exemption, staying on 32.x, or dropping Verify from `Tests.Shared`.
