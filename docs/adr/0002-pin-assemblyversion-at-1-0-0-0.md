# 0002. Pin `AssemblyVersion` at `1.0.0.0`; bump only on a breaking API change

- **Date:** 2026-08-20
- **Status:** Accepted

## Context

MSBuild defaults `AssemblyVersion` to `$(Version)` unless overridden. On the .NET Framework (and any consumer using the CLR's strong-name binding), a change to `AssemblyVersion` requires a binding redirect in the consumer's config for the reference to resolve. Minor and patch releases that bump `AssemblyVersion` therefore force every consumer that ships an `app.config` / `web.config` to add or update a `<bindingRedirect>` — a mechanical cost with zero user-visible benefit.

For a library whose target audience explicitly includes long-tail `net462` / `netstandard2.0` consumers (see [ADR-0004](0004-multi-tfm-includes-net462-and-netstandard20.md)), that cost is real.

## Decision

We will pin `AssemblyVersion` at `1.0.0.0` and only bump it on a deliberate breaking API change (a new MAJOR version per SemVer). Every other release keeps the assembly's binding identity stable. `FileVersion` and `InformationalVersion` carry the actual release version — they are the values `dotnet --info` / File Properties surface, and they do not participate in strong-name binding.

The csproj carries an explicit `<AssemblyVersion>1.0.0.0</AssemblyVersion>` with a comment naming the reason, so a future maintainer cannot delete it "as cleanup" without hitting this ADR.

## Consequences

- Consumers do not need binding redirects for minor / patch bumps — the assembly identity looks unchanged even when the shipped bits improve.
- The moment we cut a MAJOR (breaking) release we must remember to bump `AssemblyVersion` in the same PR as the code break — otherwise consumers will silently pick up a breaking change under the old identity and misdiagnose it as a runtime bug. The release-cycle checklist should call this out.
- `AssemblyInformationalVersion` (SourceLink-embedded, derived from `$(Version)`) is what shows up in stack traces, exception hosts, and package explorers — so debugging is not degraded by the identity pin.
