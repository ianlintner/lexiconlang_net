# Copilot agent instructions for `lexiconlang_net`

## Goal

Maintain a deterministic, composable, and testable .NET implementation of LexiconLang packages.

## Architecture notes

- Core deterministic primitives live in `LexiconLang.Core`.
- Higher-level packages build in layers:
  - `LexiconLang.Grammar`, `LexiconLang.Markov`, `LexiconLang.Language`
  - `LexiconLang.Glyphs`
  - `LexiconLang.Fantasy`, `LexiconLang.SciFi`, `LexiconLang.Modern`
  - `LexiconLang.Cli`
- Preserve deterministic generation behavior for seeded contexts.

## Coding conventions

- Target `net8.0`, nullable enabled, implicit usings enabled.
- Use file-scoped namespaces (`namespace X;`).
- Prefer immutable records for model objects when practical.
- Keep public APIs stable unless explicitly asked to change them.

## Determinism requirements

- Any randomness must flow through `Context<T>.Rng` / `IRng`.
- Avoid global random state.
- Preserve fork-label semantics (`ctx.Child(...)`, `Rng.Fork(...)`).

## Test expectations

- Add/adjust tests for new behavior where possible.
- Keep tests deterministic for fixed seeds.

## Build/quality expectations

- Ensure `dotnet build -warnaserror` passes.
- Keep formatting and analyzers clean (`dotnet format`).

## Packaging expectations

- Library projects should remain NuGet-packable with metadata in `Directory.Build.props`.
- Test projects and CLI should not be published as NuGet packages unless explicitly requested.
