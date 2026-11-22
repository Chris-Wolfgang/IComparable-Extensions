window.BENCHMARK_DATA = {
  "lastUpdate": 1795357661799,
  "repoUrl": "https://github.com/Chris-Wolfgang/IComparable-Extensions",
  "entries": {
    "Mutation score": [
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "fed0c416734b0fa6d74e9750c8230c691c400d47",
          "message": "security(ci): silence zizmor self-repository on the docfx call, with the reason (#299)\n\nzizmor flags `uses: ./.github/workflows/docfx.yaml` and asks for GitHub's\ndedicated `$/` self-repository syntax. We cannot switch yet: actionlint is a\nrequired check here and rejects `$/` until rhysd/actionlint#732 ships. The\nalert has therefore been sitting open with nothing to do about it.\n\nrepo-template resolved this by keeping `./` and carrying a rule-specific inline\nignore plus the reason and the exit condition. This takes the same four lines,\nso the alert closes and the next reader learns why the preferred syntax is not\nin use and when to switch.\n\nNo behaviour change: the workflow call is byte-identical apart from the comment.\n\nCo-authored-by: Claude Opus 5 <noreply@anthropic.com>",
          "timestamp": "2026-09-23T12:27:35Z",
          "url": "https://github.com/Chris-Wolfgang/IComparable-Extensions/commit/fed0c416734b0fa6d74e9750c8230c691c400d47"
        },
        "date": 1790490905169,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 100,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "fed0c416734b0fa6d74e9750c8230c691c400d47",
          "message": "security(ci): silence zizmor self-repository on the docfx call, with the reason (#299)\n\nzizmor flags `uses: ./.github/workflows/docfx.yaml` and asks for GitHub's\ndedicated `$/` self-repository syntax. We cannot switch yet: actionlint is a\nrequired check here and rejects `$/` until rhysd/actionlint#732 ships. The\nalert has therefore been sitting open with nothing to do about it.\n\nrepo-template resolved this by keeping `./` and carrying a rule-specific inline\nignore plus the reason and the exit condition. This takes the same four lines,\nso the alert closes and the next reader learns why the preferred syntax is not\nin use and when to switch.\n\nNo behaviour change: the workflow call is byte-identical apart from the comment.\n\nCo-authored-by: Claude Opus 5 <noreply@anthropic.com>",
          "timestamp": "2026-09-23T12:27:35Z",
          "url": "https://github.com/Chris-Wolfgang/IComparable-Extensions/commit/fed0c416734b0fa6d74e9750c8230c691c400d47"
        },
        "date": 1791101681336,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 100,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "dependabot[bot]",
            "username": "dependabot[bot]",
            "email": "49699333+dependabot[bot]@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "3b0b0374422057aa26f65147140ae935764aa64a",
          "message": "Bump microsoft.cst.devskim.cli from 1.0.90 to 1.0.100 (#316)\n\n---\nupdated-dependencies:\n- dependency-name: microsoft.cst.devskim.cli\n  dependency-version: 1.0.100\n  dependency-type: direct:production\n  update-type: version-update:semver-patch\n...\n\nSigned-off-by: dependabot[bot] <support@github.com>\nCo-authored-by: dependabot[bot] <49699333+dependabot[bot]@users.noreply.github.com>\nCo-authored-by: Chris Wolfgang <210299580+Chris-Wolfgang@users.noreply.github.com>",
          "timestamp": "2026-10-11T02:18:03Z",
          "url": "https://github.com/Chris-Wolfgang/IComparable-Extensions/commit/3b0b0374422057aa26f65147140ae935764aa64a"
        },
        "date": 1791700521091,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 100,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "a430044d159baa00cea23f62a5bd39e37b587837",
          "message": "fix(scripts): security-alerts updates the open summary issue instead of filing a new one (#327)\n\nGet-TrackedIssues keyed each marker to the LAST issue in the listing. The REST\nissues listing (state=all) is newest first, so with a closed old summary and an\nopen newer one the map kept the CLOSED issue, and every weekly run filed a\nduplicate summary issue. Prefer the OPEN issue for a marker, and between two of\nthe same state the higher number. Port of Chris-Wolfgang/repo-template#719.\n\nCo-authored-by: Claude Opus 5.5 <noreply@anthropic.com>",
          "timestamp": "2026-10-14T20:22:07Z",
          "url": "https://github.com/Chris-Wolfgang/IComparable-Extensions/commit/a430044d159baa00cea23f62a5bd39e37b587837"
        },
        "date": 1792305332075,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 100,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "1b191189b77aa0db87eb357fa1bc28eb355b4a57",
          "message": "test(fuzz): clear the InspectCode alerts in the fuzz project (#422)\n\nDrop two redundant using directives and compare strings with an explicit\nculture instead of string.CompareTo.\n\nCo-authored-by: Claude Sonnet 5.5 <noreply@anthropic.com>",
          "timestamp": "2026-11-07T03:27:52Z",
          "url": "https://github.com/Chris-Wolfgang/IComparable-Extensions/commit/1b191189b77aa0db87eb357fa1bc28eb355b4a57"
        },
        "date": 1794143354868,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 100,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "266c4c4181865c673b22d955c5c8d8ba3bd9c8aa",
          "message": "fix(deps): bump certifi to 2026.10.23 in the semgrep lock (GHSA-6q48-6c7w-9gw2) (#433)\n\ncertifi 2026.7.22 is in the advisory's affected range (< 2026.10.18). Regenerated\nsemgrep.txt with uv pip compile --upgrade-package certifi; certifi is the only\npackage whose version or hashes changed. Added a certifi floor to\nsemgrep-overrides.txt so a later recompile cannot resolve an affected version.\n\nCo-authored-by: Claude Opus 5.5 <noreply@anthropic.com>",
          "timestamp": "2026-11-14T15:22:14Z",
          "url": "https://github.com/Chris-Wolfgang/IComparable-Extensions/commit/266c4c4181865c673b22d955c5c8d8ba3bd9c8aa"
        },
        "date": 1794750703807,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score (tests/Wolfgang.Extensions.IComparable.Tests.Unit)",
            "value": 100,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "24cd3793b24a60ff6eef990ccd9c0b7b4d443d32",
          "message": "test(aot): delete the AotSmoke harness; Tests.Aot covers it (#468)\n\nTests.Aot links the whole Tests.Unit suite and is already the project\naot-smoke.yaml publishes and runs, so the hand-written AotSmoke console\napp only duplicated a subset of those assertions. Removes the project,\nits slnx entry and the copilot-instructions references, per the\nTests.Aot conversion plan (pilot repo).\n\nCo-authored-by: Claude Opus 5.5 <noreply@anthropic.com>",
          "timestamp": "2026-11-21T18:58:04Z",
          "url": "https://github.com/Chris-Wolfgang/IComparable-Extensions/commit/24cd3793b24a60ff6eef990ccd9c0b7b4d443d32"
        },
        "date": 1795357658020,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score (tests/Wolfgang.Extensions.IComparable.Tests.Unit)",
            "value": 100,
            "unit": "%"
          }
        ]
      }
    ]
  }
}