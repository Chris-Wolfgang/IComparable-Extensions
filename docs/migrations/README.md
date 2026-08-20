# Migration guides

One document per major-version transition. Written during release prep for the target major (not retroactively), and linked from that release's GitHub Release notes.

The template in [TEMPLATE-major-version-migration.md](TEMPLATE-major-version-migration.md) is the required shape: summary + exhaustive breaking-change inventory + before/after samples per break + deprecation timeline.

## When a new guide gets written

- The release PR that turns over the MAJOR version (0.x → 1.0, 1.x → 2.0, …) adds `vX-to-vN.md` to this folder in the same PR as the breaking code change.
- The guide is part of the release-prep checklist; a MAJOR bump does not merge without one.
- Minor / patch releases do NOT get a migration guide — non-breaking by SemVer contract.

## Index

_No guides yet — the library has not shipped a MAJOR bump. The first entry will land alongside the `v1.x → v2.0` release PR._
