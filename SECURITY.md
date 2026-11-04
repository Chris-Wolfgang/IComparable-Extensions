# Security Policy

## Reporting a Vulnerability

If you discover a security vulnerability, please follow these steps:

1. **Do not** create a public issue on this repository.
2. In the top navigation of this repository, click the **Security** tab.
3. In the top right, click the **Report a vulnerability** button.
4. Fill out the provided form with:
   - A description of the vulnerability
   - Steps to reproduce the issue
   - Potential impact
   - Suggested fix (if you have one)

## Response Timeline

We will acknowledge your report within 48 hours and provide an estimated timeline for a fix.

## Thank You

Your help is greatly appreciated!
Responsible disclosure of security vulnerabilities helps protect our entire community.

## Release path & compromise scope

Facts a maintainer would need at 2am if the release identity is compromised. Generic incident-response steps (rotating credentials, revoking OAuth apps, publishing advisories, unlisting NuGet packages) are not duplicated here — GitHub's and NuGet's own docs update faster than a checked-in runbook.

- **Release path**: OIDC / NuGet Trusted Publishing via `NuGet/login@v1` in `.github/workflows/release.yaml`. The workflow mints an ephemeral push token per run via OIDC — the release path does not depend on a long-lived API key stored in GitHub secrets or on the NuGet account. During an incident, check the NuGet account for any long-lived API keys anyway (they can be created outside of CI) and delete anything you don't recognize.
- **Fallback**: none. If Trusted Publishing is compromised, the incident is at the GitHub-account level (the OIDC identity is `Chris-Wolfgang/IComparable-Extensions`).
- **Owner**: @Chris-Wolfgang.
- **Downstream consumers**: unknown. No first-party Wolfgang.* library currently depends on `Wolfgang.Extensions.IComparable`; unknown external consumers may exist on nuget.org (public package). Use nuget.org download counts + reverse-dependency search as the incident-time signal.
- **Package coordinates for unlisting**: `Wolfgang.Extensions.IComparable` on nuget.org — https://www.nuget.org/packages/Wolfgang.Extensions.IComparable/. This repo ships exactly one package.

## Verifying the supply chain

Each release ships artifacts that let a consumer check that the package they downloaded was
built from this repository, unmodified. Replace `<version>` with the release you are checking
(`gh release download v<version> --repo Chris-Wolfgang/IComparable-Extensions` fetches the
release assets).

### 1. Package signature — `nuget verify`

NuGet.org applies a **repository signature** to every package it accepts, so this works for
every published version:

```bash
nuget verify -Signatures Wolfgang.Extensions.IComparable.<version>.nupkg
```

There is no **author signature** yet: that needs a code-signing certificate (tracked in #215).

### 2. Build provenance — SLSA attestation

`release.yaml` records a Sigstore-backed [SLSA build-provenance](https://slsa.dev/)
attestation for each `.nupkg` before it is pushed, signed with the workflow's OIDC identity
(no stored key). It binds the package's SHA-256 digest to this repository, the tagged commit
and the `release.yaml` run. Verify it with the GitHub CLI:

```bash
gh attestation verify Wolfgang.Extensions.IComparable.<version>.nupkg \
  --repo Chris-Wolfgang/IComparable-Extensions \
  --signer-workflow Chris-Wolfgang/IComparable-Extensions/.github/workflows/release.yaml
```

The same Sigstore bundle is attached to the GitHub Release as
`IComparable-Extensions-v<version>.intoto.jsonl`. Releases published before the attestation
step was added (v1.1.2 and earlier) have no attestation.

### 3. SBOM

Each release attaches a CycloneDX SBOM, `Wolfgang.Extensions.IComparable.bom.json`, listing
the package's dependency graph for vulnerability scanning and license review. The package has
no run-time package dependencies; `license-audit.yaml` checks the full transitive closure
against an allow-list on every pull request and weekly.

### 4. Reproducible build

The assemblies and symbols are byte-for-byte reproducible from the tagged source. Each
release attaches `reproducible-build-manifest.json` with their expected SHA-256 values;
[docs/REPRODUCIBLE-BUILD.md](docs/REPRODUCIBLE-BUILD.md) shows how to rebuild a tag and
compare.

Together these close the chain: **reproducible build** (the bits match the source) →
**provenance** (this repository's workflow produced the
bytes) → **signature** (nuget.org has not altered them since) → **SBOM** (what is inside).

## Security posture score

The repository's OpenSSF Scorecard score is shown by the badge in the README. Its score floor,
baseline and the findings accepted on purpose are documented in
[docs/SCORECARD.md](docs/SCORECARD.md).
