# Completed backlog items

Newest first. Open work is in [BACKLOG.md](BACKLOG.md).

### CORE-006 Triage old pull requests and branches

Completed: 2026-11-22

Closed PRs #1–#3 (from 2024) and deleted their branches; the idea from #1 is now CORE-014. Closed Dependabot PR #19 (Aspire 17, see CORE-012). Deleted the merged `features/3.0` branch.

### CORE-010 Plan the next target framework

Completed: 2026-11-22 · Decision

Move to .NET 11 in 4.0.0, `net11.0` only; 3.1.x stays on .NET 10. Tracked as CORE-012.

### CORE-008 Reconsider the Autofac dependency

Completed: 2026-11-22 · Decision

Keep Autofac in core and the reference architecture.

### CORE-007 Remove the FluentValidation dependency from Core

Completed: 2026-11-22 · Decision

Folded into CORE-013 (`Result` redesign with union types).

### Release 3.1.0-preview.3

Completed: 2026-10-03 · Tag `v3.1.0-preview.3`

Published to MyGet and nuget.org from the cleaned-up `main`.

### Clean up dependencies and bump NuGets for 3.1

Completed: 2026-10-03 · PR #4

Fixed NuGet audit failures (SourceLink, MessagePack via Aspire), removed unused and deprecated packages, bumped to EF Core 10.0.12 / NUnit 5 / StackExchange.Redis 3, pinned MediatR 12.5.0, Verify 32.x and Roslyn 5.0.0, added `global.json` and Dependabot.
