# lexiconlang_net

Minimal C#/.NET port of the Lexicon core primitives from the TypeScript
[`ianlintner/lexiconlang`](https://github.com/ianlintner/lexiconlang) project.

## Included packages

- `LexiconLang.Core`
  - Deterministic hashing + seed mixing (`Fnv1A64`, `SplitMix64`, `Mix`)
  - Seeded `Sfc32` RNG with deterministic `Fork(...)`
  - Hierarchical `Context` with child-path seeding
  - Generator abstractions and combinators (`OneOf`, `WeightedList`, `Repeat`, `Compose`, etc.)
  - Alias-method weighted sampling
- `LexiconLang.Grammar`
  - Tracery-style grammar parser + expansion and modifiers
- `LexiconLang.Markov`
  - Markov model training/sampling and generator integration
- `LexiconLang.Language`
  - Meaning packs, archetypes, phonotactics, lexicon, and templates
- `LexiconLang.Glyphs`
  - Procedural glyph generation and SVG/canvas/unicode renderers
- `LexiconLang.Fantasy`, `LexiconLang.SciFi`, `LexiconLang.Modern`
  - Content/culture packs for immediate consumption

## Consume from NuGet

Install packages (examples):

```bash
dotnet add package LexiconLang.Core
dotnet add package LexiconLang.Grammar
dotnet add package LexiconLang.Markov
dotnet add package LexiconLang.Language
dotnet add package LexiconLang.Glyphs
dotnet add package LexiconLang.Fantasy
dotnet add package LexiconLang.SciFi
dotnet add package LexiconLang.Modern
```

## Build and test

```bash
dotnet build -warnaserror
dotnet test LexiconLang.Net.slnx
```

## CI and release automation

- `CI` workflow (`.github/workflows/ci.yml`)
  - Restore, format verification, build with warnings-as-errors, and test.
- `Pack & Publish NuGet` workflow (`.github/workflows/pack.yml`)
  - Packs all library projects on tag pushes (`v*`) or manual dispatch.
  - Supports an optional manual `version` input for preview or dry-run package builds.
  - Uploads `.nupkg` + `.snupkg` artifacts.
  - Publishes to NuGet when run on a version tag and `NUGET_API_KEY` secret is configured.

### Release flow

1. Create a git tag like `v0.1.0`.
2. Push the tag.
3. Workflow packs and publishes packages to NuGet.org.
4. For a maintainer checklist and dry-run flow, see [`RELEASE.md`](RELEASE.md).

### Required GitHub secret

- `NUGET_API_KEY`: API key with package push rights for NuGet.org.
