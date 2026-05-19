# lexiconlang_net

Minimal C#/.NET port of the Lexicon core primitives from the TypeScript
[`ianlintner/lexiconlang`](https://github.com/ianlintner/lexiconlang) project.

## Included in this port

- Deterministic hashing + seed mixing (`Fnv1A64`, `SplitMix64`, `Mix`)
- Seeded `Sfc32` RNG with deterministic `Fork(...)`
- Hierarchical `Context` with child-path seeding
- Generator abstractions and combinators (`OneOf`, `WeightedList`, `Repeat`, `Compose`, etc.)
- Alias-method weighted sampling

## Build and test

```bash
dotnet test LexiconLang.Core.Tests/LexiconLang.Core.Tests.csproj
```
