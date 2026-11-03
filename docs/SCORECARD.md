# OpenSSF Scorecard

[OpenSSF Scorecard](https://github.com/ossf/scorecard) scores this repository's security posture
(branch protection, pinned dependencies, token permissions, dangerous workflow patterns, and so on)
from 0 to 10. It scores the repository's configuration, not the library code; CodeQL, Semgrep and
DevSkim cover the code.

## How it runs

`.github/workflows/scorecard.yaml` runs `ossf/scorecard-action` weekly, on every push to `main`,
and when branch protection changes. It does two things:

- **Publishes the score** to scorecard.dev (`publish_results: true`). The README badge reads it from
  `https://api.scorecard.dev/projects/github.com/Chris-Wolfgang/IComparable-Extensions`. scorecard.dev
  only accepts a result from a job whose steps are all `uses:`, so the analysis job does nothing
  else (#326).
- **Uploads the findings** to the Security tab (Code Scanning, tool "Scorecard") after a separate job
  drops the suppressed findings listed below.

The current score and every check's detail are at
<https://scorecard.dev/viewer/?uri=github.com/Chris-Wolfgang/IComparable-Extensions>.

## Score floor

The floor is **7.5**. A change that drops the published score below 7.5, or that lowers it by a
full point or more, needs a line in that release's changelog fragment saying which check dropped and
why that is accepted. Scorecard runs on `main`, not on pull requests, so the drop shows up on the
first run after the merge. Whoever reviews the next release checks the badge against the floor.

## Baseline

**Initial score:** not yet recorded. Every run before #326 was fixed was refused by scorecard.dev
(`scorecard job must only have steps with 'uses'`), so the API returned 404 and no score exists.
Record the first published score and its date here after the first run on `main` that includes
#326's fix.

**Known, accepted findings.** These lower the score on purpose:

| Check | Status | Why it is accepted |
|---|---|---|
| Branch-Protection (`BranchProtectionID`) | Open alert, tracked in #274 (labelled `blocked`) | Scorecard's higher tiers need required approving reviews, which a solo-maintainer repository can only meet with an admin bypass on every merge. |
| Code-Review (`CodeReviewID`) | Suppressed in `scorecard.yaml` | Solo maintainer; requiring a second approver would force an admin bypass on every pull request. |
| CII-Best-Practices (`CIIBestPracticesID`) | Suppressed in `scorecard.yaml` | The bestpractices.dev badge is an external registration, not something CI can earn. |
| Pinned-Dependencies, restore commands only (`nugetCommand not pinned` on `dotnet restore`) | Suppressed in `scorecard.yaml` | Every project pins exact versions and nuget.org packages are immutable; lock-file restore broke every pull request on each SDK patch (fleet decision 2026-09-16). An unpinned `dotnet add package` / `nuget install` is still reported. |
| Signed-Releases | Not a Code Scanning alert | Releases attach a SLSA provenance bundle (`*.intoto.jsonl`, #163). A NuGet author signature needs a code-signing certificate (#215). |

Suppressions are applied by the "Filter suppressed findings from SARIF" step, never by dismissing
alerts one at a time, because alert fingerprints drift and dismissed alerts come back under new
IDs. The suppressions hide findings from the Security tab only; the published score still counts
them.

Code Scanning history for the Scorecard tool, read 2026-10-30: `BranchProtectionID` open (alert #101),
`SASTID` fixed. No other Scorecard rule has raised an alert in this repository.
