# Reproducible Builds

`Wolfgang.Extensions.IComparable` is built **deterministically**: compiling the same commit
**with the same .NET SDK** produces byte-for-byte identical assemblies and symbols, on Linux
and Windows alike and regardless of the machine or working directory. That lets anyone
confirm independently that a published package was built from the tagged source and
contains nothing else.

## The guarantee

For a given commit built with the same SDK, these outputs are bit-for-bit identical:

- `Wolfgang.Extensions.IComparable.dll` for every target framework (`net462`,
  `netstandard2.0`, `net8.0`, `net10.0`), as shipped in the `.nupkg`,
- `Wolfgang.Extensions.IComparable.pdb` for every target framework, as shipped in the
  `.snupkg`.

> **The package files themselves are not byte-reproducible.** A `.nupkg` / `.snupkg` is a
> zip, and NuGet stamps every entry with the time the package was packed (and writes
> platform-specific metadata), so packing the same commit twice gives two different package
> files even on one machine. Compare what is *inside* them, not the package file.

Releases published after the manifest step was added to `release.yaml` carry the manifest
described below. v1.1.2 and earlier predate the cross-OS fixes and have no manifest.

## How it is achieved

| Setting | Where | Effect |
| --- | --- | --- |
| `Deterministic` (SDK default `true`) | SDK | No timestamps or other non-deterministic metadata in the assembly. |
| `ContinuousIntegrationBuild=true` when `CI=true` | `Directory.Build.props` | Turns on `DeterministicSourcePaths`, replacing absolute source paths with stable tokens. |
| `Microsoft.SourceLink.GitHub` + `EmbedUntrackedSources` | `Directory.Build.props` | Maps source paths to the repository, which keeps the PDB machine-independent. |
| `NormalizeGeneratedLineEndings` target | `Directory.Build.targets` | Rewrites the SDK-generated `*.AssemblyInfo.cs` / `*.AssemblyAttributes.cs` to LF before compiling. They are written with the OS newline, and the PDB records their hash and text, so without this the PDB (and through its checksum the DLL) differed between Windows and Linux. |
| `Microsoft.NETFramework.ReferenceAssemblies` for `net4*` | `Directory.Build.props` | Every OS compiles `net462` against the same reference assemblies instead of Windows using the installed targeting pack. The PDB records each reference's identity. |

## Continuous verification

The [`Reproducible Build`](../.github/workflows/reproducible-build.yaml) workflow runs on
every pull request and push to `main`: it packs the project on `ubuntu-latest` and
`windows-latest` with `CI=true`, hashes every `.dll` and `.pdb`, and **fails** if any hash
differs between the two operating systems.

## The release manifest

Every release attaches **`reproducible-build-manifest.json`**, generated inside
`release.yaml` from the packages that were published. It records the `repository`, `tag`,
`commit` and `sdk` version, and for each package the SHA-256 of every `.dll` / `.pdb` it
contains (`packages[].files[]`). The package files' own `sha256` is informational only (see
above).

## Verify it yourself

Use the SDK version recorded in the manifest (`sdk`); a different compiler can legitimately
emit different, still deterministic, bytes.

```bash
VERSION='<version>'   # the release you are checking, without the leading v

# 1. The tagged source, packed deterministically.
git clone https://github.com/Chris-Wolfgang/IComparable-Extensions.git
cd IComparable-Extensions
git checkout "v$VERSION"
CI=true dotnet pack src/Wolfgang.Extensions.IComparable/Wolfgang.Extensions.IComparable.csproj \
  -c Release -p:ContinuousIntegrationBuild=true -p:Deterministic=true -o artifacts

# 2. Hash the assemblies and symbols inside your packages.
mkdir -p mine
unzip -oq "artifacts/Wolfgang.Extensions.IComparable.$VERSION.nupkg"  -d mine
unzip -oq "artifacts/Wolfgang.Extensions.IComparable.$VERSION.snupkg" -d mine
# (sed drops the '*' binary-mode marker some sha256sum builds print, e.g. Git for Windows.)
(cd mine && find lib -type f \( -name '*.dll' -o -name '*.pdb' \) -exec sha256sum {} + | sed 's/ \*/  /' | sort -k2) > mine.txt

# 3. The published expectations, in the same format.
gh release download "v$VERSION" --repo Chris-Wolfgang/IComparable-Extensions \
  --pattern reproducible-build-manifest.json
jq -r '.packages[].files[] | "\(.sha256)  \(.path)"' reproducible-build-manifest.json | tr -d '\r' | sort -k2 > released.txt

diff released.txt mine.txt && echo "Reproduced: identical assemblies and symbols."
```

An empty `diff` shows that the assemblies and symbols you rebuilt from source are
byte-identical to the published ones. To check the package you actually installed, unzip it
from your NuGet cache and hash its `lib/` entries the same way.

## File a discrepancy

A hash difference with the SDK version from the manifest is a supply-chain signal. Please
[open an issue](https://github.com/Chris-Wolfgang/IComparable-Extensions/issues/new) titled
`Reproducible-build discrepancy: v<version>` with:

- the release tag and the manifest's `sdk` value,
- your OS / architecture and `dotnet --version`,
- `released.txt`, `mine.txt` and the `diff` output.

Discrepancies are handled as security reports; see [SECURITY.md](../SECURITY.md).

## Publish a third-party attestation

Independent verification is most useful when it is public. If you have reproduced a
release:

- record it following the [Reproducible Builds](https://reproducible-builds.org/)
  project's conventions for independent rebuilds, or publish a signed statement (for example
  an in-toto attestation, or via a service such as [vouchsafe.io](https://vouchsafe.io/))
  that names the release tag and the hashes you confirmed, and
- link it from an issue titled `Reproducible-build attestation: v<version>`, so later
  consumers can see that the release was verified independently.
