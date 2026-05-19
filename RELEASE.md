# Release guide

This repository publishes NuGet packages from Git tags through `.github/workflows/pack.yml`.

## Prerequisites

- GitHub Actions secret `NUGET_API_KEY` is configured for this repository.
- The `CI` workflow is green on `main`.
- You are releasing from the commit you intend to publish.

## Dry-run package build

Use the `Pack & Publish NuGet` workflow with **Run workflow** and optionally provide a `version` input such as `0.2.0-preview.1`.

This will:

- build in `Release`
- pack all library projects
- upload `.nupkg` and `.snupkg` artifacts
- skip NuGet publish unless the run comes from a `v*` tag

## Production release

1. Update any version notes or changelog entries you want in the GitHub release.
2. Create and push a tag in the form `vX.Y.Z`.
3. Confirm the `Pack & Publish NuGet` workflow succeeds.
4. Verify the expected packages appear on NuGet.org.

## What gets published

Library packages only:

- `LexiconLang.Core`
- `LexiconLang.Grammar`
- `LexiconLang.Markov`
- `LexiconLang.Language`
- `LexiconLang.Glyphs`
- `LexiconLang.Fantasy`
- `LexiconLang.SciFi`
- `LexiconLang.Modern`

The CLI and test projects are intentionally not packable.

## If a publish partially succeeds

The publish step uses `--skip-duplicate`, so rerunning the workflow is safe for packages already pushed.

If you need to retry:

- fix the workflow or repository issue
- rerun the workflow for the same tag, or delete and recreate the tag if needed

## Suggested first release

If this is the first public package publish, a conservative first tag would be:

`v1.0.0`
