# LexiconLang: TypeScript ↔ .NET API Quick Reference

This document maps TypeScript APIs to their .NET equivalents for developers porting code or comparing implementations.

---

## 1. Core Module APIs

### RNG & Seeding

```typescript
// TypeScript
import { createRng, Sfc32, type Seed } from "@lexiconlang/core";

const rng1 = createRng("seed-string");
const rng2 = createRng(42);
const rng3 = createRng(12345n);
const rng4 = createRng({ algo: "sfc32", state: [...], origin: "..." });
```

```csharp
// .NET
using LexiconLang.Core;

var rng1 = new Sfc32("seed-string");
var rng2 = new Sfc32(42L);
var rng3 = new Sfc32(12345UL);
var rng4 = new Sfc32(state);
```

### RNG Operations

| TypeScript                        | .NET                              |
| --------------------------------- | --------------------------------- |
| `rng.next(): number [0,1)`        | `rng.Next(): double [0,1)`        |
| `rng.nextU32(): number`           | `rng.NextUInt32(): uint`          |
| `rng.nextInt(min, max)`           | `rng.NextInt(min, maxExclusive)`  |
| `rng.nextRange(min, max): number` | `rng.NextRange(min, max): double` |
| `rng.pick(items)`                 | `rng.Pick(items)`                 |
| `rng.state(): RngState`           | `rng.State(): RngState`           |
| `rng.fork("label")`               | `rng.Fork("label")`               |
| `rng.fork(0)`                     | `rng.Fork(0)`                     |

### Context Creation & Usage

```typescript
// TypeScript
import { createContext, type Context } from "@lexiconlang/core";

const ctx = createContext({
  seed: "my-seed",
  tags: ["tag1", "tag2"],
  locale: "en-US",
  data: { custom: "payload" },
  registry: myRegistry,
});

const child = ctx.child("label", { tags: ["new-tag"] });
const withTags = ctx.withTags("additional");
const withData = ctx.withData({ newData: true });
```

```csharp
// .NET
using LexiconLang.Core;

var ctx = new Context<MyData>(
  rng: new Sfc32("my-seed"),
  scope: new[] { },
  tags: new HashSet<string> { "tag1", "tag2" },
  data: new MyData { Custom = "payload" },
  locale: "en-US",
  registry: myRegistry
);

// Or use helper (if exists):
var child = ctx.Child("label", new ContextOverrides<MyData> { Tags = ["new-tag"] });
var withTags = ctx.WithTags("additional");
var withData = ctx.WithData(new MyData { NewData = true });
```

### Combinators

```typescript
// TypeScript
import {
  oneOf,
  pickOf,
  constant,
  repeat,
  compose,
  map,
  chain,
  intRange,
  floatRange,
  weightedList,
} from "@lexiconlang/core";

const genA = oneOf("a", "b", "c");
const genB = pickOf(["x", "y"], [2, 1]); // 2:1 ratio
const genC = constant("fixed");
const genD = repeat(genA, 3); // Always 3
const genE = repeat(genA, [1, 3]); // Range 1-3
const genF = compose(genA, genB, { separator: "-" });
const genG = map(genA, (s) => s.toUpperCase());
const genH = chain(genA, (val) => oneOf(val, "fallback"));
const genI = intRange(1, 100);
const genJ = floatRange(0.0, 1.0);
const genK = weightedList([
  { value: genA, weight: 3 },
  { value: genB, weight: 1 },
]);
```

```csharp
// .NET
using LexiconLang.Core;

var genA = Combinators.OneOf("a", "b", "c");
var genB = Combinators.PickOf(new[] { "x", "y" }, new double[] { 2, 1 });
var genC = Combinators.Constant("fixed");
var genD = Combinators.Repeat(genA, 3);
var genE = Combinators.Repeat(genA, (1, 3));
var genF = Combinators.Compose(new[] { genA, genB }, separator: "-");
var genG = Combinators.Map(genA, s => s.ToUpper());
var genH = Combinators.Chain(genA, val => Combinators.OneOf(val, "fallback"));
var genI = Combinators.IntRange(1, 100);
var genJ = Combinators.FloatRange(0.0, 1.0);
var genK = Combinators.WeightedList(new[] {
  new { Value = (IGenerator<string>)genA, Weight = 3.0 },
  new { Value = (IGenerator<string>)genB, Weight = 1.0 }
});
```

### Weighted Sampling

```typescript
// TypeScript
import {
  buildAliasTable,
  sampleAlias,
  normalizeWeights,
} from "@lexiconlang/core";

const normalized = normalizeWeights(["a", "b", "c"]); // All weight 1
const aliasTable = buildAliasTable(normalized);
const sample = sampleAlias(aliasTable, rng);
```

```csharp
// .NET
using LexiconLang.Core;

var normalized = Sample.NormalizeWeights(new[] { "a", "b", "c" });
var aliasTable = Sample.BuildAliasTable(normalized);
var sample = Sample.SampleAlias(aliasTable, rng);
```

### Hashing & Utilities

```typescript
// TypeScript
import { fnv1a64, splitmix64, mix, seedToBigInt } from "@lexiconlang/core";

const hash1 = fnv1a64("input");
const hash2 = splitmix64(123n);
const hash3 = mix(123n, "label");
const bigint = seedToBigInt("seed");
```

```csharp
// .NET
using LexiconLang.Core;

var hash1 = Hashing.Fnv1a64("input");
var hash2 = Hashing.SplitMix64(123UL);
var hash3 = Hashing.Mix(123UL, "label");
var bigint = Hashing.SeedToUInt64("seed");
```

---

## 2. Grammar Module APIs

### Grammar Definition & Expansion

```typescript
// TypeScript
import { grammar, type Grammar, type GrammarRules } from "@lexiconlang/grammar";

const rules: GrammarRules = {
  start: { "#adjective# #noun#": 2, "#noun#": 1 },
  adjective: ["big", "small", "red"],
  noun: ["cat", "dog"],
};

const gen: Grammar = grammar(rules, {
  id: "my-grammar",
  start: "start",
  maxDepth: 32,
  modifiers: { custom: customModifier },
  registry: myRegistry,
});

const result = gen.generate(ctx);
const expanded = gen.expand("#start#", ctx);
```

```csharp
// .NET
using LexiconLang.Grammar;
using LexiconLang.Core;

var rules = new GrammarRules<MyData>
{
    { "start", new { Weight = 2, Value = "#adjective# #noun#", Weight = 1, Value = "#noun#" } },
    { "adjective", new[] { "big", "small", "red" } },
    { "noun", new[] { "cat", "dog" } }
};

var gen = new Grammar<MyData>(rules, new GrammarOptions<MyData>
{
    Id = "my-grammar",
    Start = "start",
    MaxDepth = 32,
    Modifiers = new() { { "custom", customModifier } },
    Registry = myRegistry
});

var result = gen.Generate(ctx);
var expanded = gen.Expand("#start#", ctx);
```

### Template Syntax

| Feature            | TypeScript               | .NET                     |
| ------------------ | ------------------------ | ------------------------ |
| Rule ref           | `#rule#`                 | `#rule#`                 |
| Weighted choice    | `#[opt1:2, opt2:1]#`     | `#[opt1:2, opt2:1]#`     |
| Modifier           | `#rule \| modifier#`     | `#rule \| modifier#`     |
| Multiple modifiers | `#rule \| mod1 \| mod2#` | `#rule \| mod1 \| mod2#` |
| Escape             | `\#` → `#`               | `\#` → `#`               |

### Built-in Modifiers

| TypeScript    | .NET         |
| ------------- | ------------ |
| `capitalize`  | `capitalize` |
| `uppercase`   | `uppercase`  |
| `lowercase`   | `lowercase`  |
| `reverse`     | `reverse`    |
| `a` (article) | `a`          |
| `s` (plural)  | `s`          |

### Custom Modifiers

```typescript
// TypeScript
interface Modifier {
  id: string;
  apply(value: string, ctx: Context): string;
}

const customMod: Modifier = {
  id: "custom",
  apply(value: Context) {
    return value.toUpperCase().split("").reverse().join("");
  },
};
```

```csharp
// .NET
public interface IModifier
{
    string Id { get; }
    string Apply(string value, Context<TData> ctx);
}

public class CustomModifier : IModifier
{
    public string Id => "custom";
    public string Apply(string value, Context<TData> ctx) =>
        new string(value.ToUpper().Reverse().ToArray());
}
```

### Parser AST

```typescript
// TypeScript
import { parse, type Node } from "@lexiconlang/grammar";

const ast = parse("#rule | mod#");
// Returns: Node[] with ModifierCall | string nodes
```

```csharp
// .NET
using LexiconLang.Grammar;

var ast = GrammarParser.Parse("#rule | mod#");
// Returns: ??? (check implementation)
```

---

## 3. Language Module APIs

### Glyph System & Phonotactics

```typescript
// TypeScript
import { generateWord, type GlyphSystem } from "@lexiconlang/language";

const glyphs: GlyphSystem = {
  classes: {
    C: ["b", "c", "d"],
    V: ["a", "e", "i"],
  },
  syllables: [
    ["C V", 2],
    ["C V C", 1],
  ],
  wordShapes: [
    ["1", 1],
    ["2", 1],
  ],
  constraints: [
    { pattern: ["C", "C"], rule: "forbid" },
    { pattern: ["V", "*", "V"], rule: { maxOccurrences: 1 } },
  ],
  joiner: "",
};

const word = generateWord(glyphs, ctx);
```

```csharp
// .NET
using LexiconLang.Language;
using LexiconLang.Core;

var glyphs = new GlyphSystem
{
    Classes = new()
    {
        { "C", new[] { "b", "c", "d" } },
        { "V", new[] { "a", "e", "i" } }
    },
    Syllables = new[]
    {
        ("C V", 2.0),
        ("C V C", 1.0)
    },
    WordShapes = new[]
    {
        ("1", 1.0),
        ("2", 1.0)
    },
    Constraints = new[]
    {
        new Constraint { Pattern = new[] { "C", "C" }, Rule = "forbid" },
        new Constraint { Pattern = new[] { "V", "*", "V" }, Rule = new { MaxOccurrences = 1 } }
    },
    Joiner = ""
};

var word = Phonotactics.GenerateWord(glyphs, ctx);
```

### Lexicon Building

```typescript
// TypeScript
import {
  buildLexicon,
  type Lexicon,
  type Culture,
} from "@lexiconlang/language";

const lex: Lexicon = buildLexicon(culture, ctx);
const form = lex.formOf("meaning.id");
const nouns = lex.byClass("noun", "combat");
const all = lex.materialize();
```

```csharp
// .NET
using LexiconLang.Language;

var lex = Lexicon.BuildLexicon(culture, ctx);
var form = lex.FormOf("meaning.id");
var nouns = lex.ByClass("noun", "combat");
var all = lex.Materialize();
```

### Culture Definition

```typescript
// TypeScript
import { type Culture } from "@lexiconlang/language";

const culture: Culture = {
  id: "elvish",
  glyphs: glyphSystem,
  meaningPacks: [coreMeanings, elvishMeanings],
  templates: nameTemplates,
  conjugations: { past: pastGrammar },
};
```

```csharp
// .NET
using LexiconLang.Language;

var culture = new Culture
{
    Id = "elvish",
    Glyphs = glyphSystem,
    MeaningPacks = new[] { coreMeanings, elvishMeanings },
    Templates = nameTemplates,
    Conjugations = new() { { "past", pastGrammar } }
};
```

### Name Generation

```typescript
// TypeScript
import { generateName, type TranslatedName } from "@lexiconlang/language";

const name: TranslatedName = generateName(culture, "noun", ctx);
// { form: "elvír", translation: "star-light", details: {...} }
```

```csharp
// .NET
using LexiconLang.Language;

var name = Templates.GenerateName(culture, "noun", ctx);
// TranslatedName { Form = "elvír", Translation = "star-light", Details = {...} }
```

### Meanings & Types

```typescript
// TypeScript
import { type Meaning, type WordClass } from "@lexiconlang/language";

const meaning: Meaning = {
  id: "star",
  class: "noun",
  tags: ["celestial", "nature"],
  label: "A celestial body",
};

type WordClass = "noun" | "adjective" | "verb" | "particle";
```

```csharp
// .NET
using LexiconLang.Language;

var meaning = new Meaning
{
    Id = "star",
    Class = "noun",
    Tags = new[] { "celestial", "nature" },
    Label = "A celestial body"
};

// WordClass is likely an enum or const strings
```

---

## 4. Markov Module APIs

### Training

```typescript
// TypeScript
import { train, type TrainOptions, type Corpus } from "@lexiconlang/markov";

const corpus1: Corpus = ["elara", "elowen", "evandir"];
const corpus2: Corpus = [
  { word: "elara", weight: 2 },
  { word: "elowen", weight: 1 },
];

const model = train(corpus1, {
  order: 3,
  minLength: 4,
  maxLength: 12,
  rejectSubstringsOfLength: 5,
  pruneBelow: 1,
  lowercase: true,
  meta: { source: "elvish" },
});
```

```csharp
// .NET
using LexiconLang.Markov;

var corpus1 = new[] { "elara", "elowen", "evandir" };
var corpus2 = new[]
{
    new TrainEntry { Word = "elara", Weight = 2.0 },
    new TrainEntry { Word = "elowen", Weight = 1.0 }
};

var model = Trainer.Train(corpus1, new TrainOptions
{
    Order = 3,
    MinLength = 4,
    MaxLength = 12,
    RejectSubstringsOfLength = 5,
    PruneBelow = 1,
    Lowercase = true,
    Meta = new() { { "source", "elvish" } }
});
```

### Sampling

```typescript
// TypeScript
import { sample, markov, type SampleOptions } from "@lexiconlang/markov";

const str1 = sample(model, rng, {
  maxAttempts: 200,
  minLength: 4,
  maxLength: 12,
  capitalize: true,
});

const gen = markov(model, { id: "markov.elvish" });
const str2 = gen.generate(ctx);
```

```csharp
// .NET
using LexiconLang.Markov;

var str1 = Sampler.Sample(model, rng, new SampleOptions
{
    MaxAttempts = 200,
    MinLength = 4,
    MaxLength = 12,
    Capitalize = true
});

var gen = Sampler.Markov(model, new MarkovGeneratorOptions { Id = "markov.elvish" });
var str2 = gen.Generate(ctx);
```

### Model Serialization

```typescript
// TypeScript
import { toJSON, fromJSON } from "@lexiconlang/markov";

const json = toJSON(model);
const restored = fromJSON(json);
```

```csharp
// .NET
using LexiconLang.Markov;
using System.Text.Json;

var json = JsonSerializer.Serialize(model);
var restored = JsonSerializer.Deserialize<MarkovModel>(json);
```

---

## 5. Glyphs Module APIs

### Glyph Generation

```typescript
// TypeScript
import {
  glyphsFor,
  type VisualGlyphSystem,
  type TranslatedName,
  type GlyphSet,
} from "@lexiconlang/glyphs";

const system: VisualGlyphSystem = {
  id: "elvish-alphabet",
  type: "alphabet",
  renderFormat: "svg",
  mappingStrategy: "phoneme",
  generator: {
    baseShapes: ["rect", "circle", "line"],
    complexity: "medium",
    symmetry: true,
    palette: ["#333", "#666"],
  },
};

const glyphSet: GlyphSet = glyphsFor(name, system, ctx);
```

```csharp
// .NET
using LexiconLang.Glyphs;
using LexiconLang.Language;

var system = new VisualGlyphSystem
{
    Id = "elvish-alphabet",
    Type = "alphabet",
    RenderFormat = "svg",
    MappingStrategy = "phoneme",
    Generator = new()
    {
        BaseShapes = new[] { "rect", "circle", "line" },
        Complexity = "medium",
        Symmetry = true,
        Palette = new[] { "#333", "#666" }
    }
};

var glyphSet = GlyphGenerator.GlyphsFor(name, system, ctx);
```

### Shape Generation

```typescript
// TypeScript
import { generateShapes } from "@lexiconlang/glyphs";

const shapes = generateShapes(ctx);
// Returns SVG path strings or canvas instructions
```

```csharp
// .NET
using LexiconLang.Glyphs;

var shapes = ShapeGenerator.GenerateShapes(ctx);
```

### Rendering

```typescript
// TypeScript
import {
  renderToSVG,
  renderToUnicode,
  renderToCanvas,
} from "@lexiconlang/glyphs";

const svg = renderToSVG(glyph, { size: 64, strokeWidth: 2 });
const unicode = renderToUnicode(glyph, { fallback: "?" });
const canvas = renderToCanvas(glyph, { size: 128 });
```

```csharp
// .NET
using LexiconLang.Glyphs;

var svg = SvgRenderer.RenderToSVG(glyph, new RenderParams { Size = 64, StrokeWidth = 2 });
var unicode = UnicodeRenderer.RenderToUnicode(glyph, new UnicodeConfig { Fallback = "?" });
var canvas = CanvasRenderer.RenderToCanvas(glyph, new RenderParams { Size = 128 });
```

---

## 6. Content Pack APIs

### Fantasy

```typescript
// TypeScript
import {
  dwarvish,
  elvish,
  orcish,
  halfling,
  draconic,
  plantoid,
  mycanoids,
  celestial,
  fey,
  tiefling,
} from "@lexiconlang/fantasy";

const dwarfName = generateName(dwarvish, "noun", ctx);
```

```csharp
// .NET
using LexiconLang.Fantasy;

var dwarfName = Templates.GenerateName(FantasyCultures.Dwarvish, "noun", ctx);
```

### SciFi

```typescript
// TypeScript
import {
  humanoid,
  insectoid,
  aquatic,
  synth,
  birdpeople,
  rockpeople,
  mycoids,
  mammalian,
  plantoid,
  reptilian,
  hivemind,
  grayfolk,
  humanoidName,
  insectoidName,
  aquaticName,
} from "@lexiconlang/scifi";

const alienName = humanoidName.generate(ctx);
```

```csharp
// .NET
using LexiconLang.SciFi;

var alienName = SciFiGenerators.HumanoidName.Generate(ctx);
```

### Modern

```typescript
// TypeScript
import {
  personName,
  cityName,
  streetName,
  companyName,
  givenMaleName,
  givenFemaleName,
  surname,
} from "@lexiconlang/modern";

const person = personName.generate(ctx);
const city = cityName.generate(ctx);
```

```csharp
// .NET
using LexiconLang.Modern;

var person = ModernGenerators.PersonName.Generate(ctx);
var city = ModernGenerators.CityName.Generate(ctx);
```

---

## 7. CLI APIs

### Command Structure

```typescript
// TypeScript
// CLI: lexiconlang <command> [options]
// lexiconlang build-markov input.json --out model.json --order 3
// lexiconlang scaffold-pack my-pack --dir packages
```

```csharp
// .NET (expected)
// CLI: dotnet lexiconlang-cli.dll <command> [options]
// Or if packaged as global tool:
// lexiconlang build-markov input.json --out model.json --order 3
// lexiconlang scaffold-pack my-pack --dir packages
```

---

## 8. Type Mapping Cheat Sheet

| Concept        | TypeScript            | .NET                            |
| -------------- | --------------------- | ------------------------------- | ---- | ----- | --------- |
| Seed           | `Seed` (union type)   | `string                         | long | ulong | RngState` |
| RNG            | `RNG` (interface)     | `IRng` (interface)              |
| Context        | `Context<S>`          | `Context<TData>`                |
| Generator      | `Generator<T, C>`     | `IGenerator<T, TContextData>`   |
| Weighted value | `Weighted<T>`         | `Weighted<T>` record            |
| Weighted input | `WeightInput<T>`      | Various (tuples, records, etc.) |
| Grammar rules  | `GrammarRules<C>`     | `GrammarRules<TContextData>`    |
| Meaning        | `Meaning`             | `Meaning` record/class          |
| Culture        | `Culture`             | `Culture` class                 |
| Lexicon        | `Lexicon` (interface) | `Lexicon` class                 |
| TranslatedName | `TranslatedName`      | `TranslatedName` record/class   |
| GlyphSystem    | `GlyphSystem`         | `GlyphSystem` class/record      |
| Glyph          | `Glyph`               | `Glyph` record/class            |
| MarkovModel    | `MarkovModel`         | `MarkovModel` class/record      |

---

## 9. Common Patterns

### Pattern: Deterministic Name Generation

```typescript
// TypeScript
const ctx = createContext({ seed: "deterministic-seed" });
const ctx2 = createContext({ seed: "deterministic-seed" });

const name1 = generateName(culture, "noun", ctx.child("names"));
const name2 = generateName(culture, "noun", ctx2.child("names"));
// name1.form === name2.form ✓
```

```csharp
// .NET
var ctx = new Context<object>(
    rng: new Sfc32("deterministic-seed"),
    scope: Array.Empty<string>(),
    tags: new HashSet<string>(),
    data: null!
);
var ctx2 = new Context<object>(
    rng: new Sfc32("deterministic-seed"),
    scope: Array.Empty<string>(),
    tags: new HashSet<string>(),
    data: null!
);

var name1 = Templates.GenerateName(culture, "noun", ctx.Child("names"));
var name2 = Templates.GenerateName(culture, "noun", ctx2.Child("names"));
// name1.Form == name2.Form ✓
```

### Pattern: Custom Generator

```typescript
// TypeScript
const myGen: Generator<string> = {
  id: "custom.generator",
  generate(ctx: Context) {
    return ctx.rng.pick(["option1", "option2", "option3"]);
  },
};
```

```csharp
// .NET
public class MyGenerator : IGenerator<string, object>
{
    public string Id => "custom.generator";
    public string Generate(Context<object> ctx) =>
        ctx.Rng.Pick(new[] { "option1", "option2", "option3" });
}

var myGen = new MyGenerator();
```

### Pattern: Composition

```typescript
// TypeScript
const compound = compose(
  oneOf("blue", "red", "green"),
  oneOf("dragon", "eagle"),
  { separator: "-" },
);
// Generates: "blue-dragon", "red-eagle", etc.
```

```csharp
// .NET
var compound = Combinators.Compose(
    new IGenerator<string>[] {
        Combinators.OneOf("blue", "red", "green"),
        Combinators.OneOf("dragon", "eagle")
    },
    separator: "-"
);
```

---

## 10. Testing Equivalents

### Determinism Test

```typescript
// TypeScript (Vitest)
import { describe, it, expect } from "vitest";
import { createContext, generateWord } from "@lexiconlang/language";

describe("Determinism", () => {
  it("same seed produces same output", () => {
    const ctx1 = createContext({ seed: "test" });
    const ctx2 = createContext({ seed: "test" });
    expect(generateWord(glyphs, ctx1.child("w"))).toBe(
      generateWord(glyphs, ctx2.child("w")),
    );
  });
});
```

```csharp
// .NET (xUnit / NUnit)
using Xunit;
using LexiconLang.Core;
using LexiconLang.Language;

public class DeterminismTests
{
    [Fact]
    public void SameSeedProducesSameOutput()
    {
        var ctx1 = new Context<object>(
            rng: new Sfc32("test"),
            scope: Array.Empty<string>(),
            tags: new HashSet<string>(),
            data: null!
        );
        var ctx2 = new Context<object>(
            rng: new Sfc32("test"),
            scope: Array.Empty<string>(),
            tags: new HashSet<string>(),
            data: null!
        );

        Assert.Equal(
            Phonotactics.GenerateWord(glyphs, ctx1.Child("w")),
            Phonotactics.GenerateWord(glyphs, ctx2.Child("w"))
        );
    }
}
```

---

## 11. Known API Differences

| Aspect    | TypeScript     | .NET               | Reason                  |
| --------- | -------------- | ------------------ | ----------------------- |
| Naming    | camelCase      | PascalCase         | .NET convention         |
| Unions    | `type A \| B`  | Generic overloads  | Type system             |
| Null      | `undefined`    | `null`             | C# standard             |
| Arrays    | `readonly T[]` | `IReadOnlyList<T>` | .NET idiomatic          |
| Records   | ES6 class      | `record`           | C# 9+ feature           |
| Iterables | `Iterable<T>`  | `IEnumerable<T>`   | .NET standard           |
| Dicts     | `Record<K, V>` | `Dictionary<K, V>` | .NET standard           |
| Async     | No (sync)      | No (sync)          | Determinism requirement |

---

## 12. Troubleshooting

### "Context not inferring type correctly"

**TypeScript Issue**: Often due to generic narrowing

```typescript
// Narrow it explicitly:
const ctx: Context<MyData> = createContext({ seed: "...", data: myData });
```

**C# Issue**: Use explicit type parameter

```csharp
// Specify type explicitly:
var ctx = new Context<MyData>(...);
```

### "Weighted list not being applied"

**TypeScript**:

```typescript
// Make sure you use weighted format:
const rules = {
  start: { "#a#": 2, "#b#": 1 }, // ✓ Weighted
  // NOT: start: ["#a#", "#a#", "#b#"]  // ✗ Duplicate style
};
```

**C#**:

```csharp
// Same rule applies:
var rules = new GrammarRules<T>
{
    { "start", new { Weight = 2.0, Value = "#a#" /* or similar */ } }  // Check syntax
};
```

### "Determinism not working"

**Both**:

- Use same seed for both contexts
- Don't modify context data between calls
- Use child forks consistently
- Verify child labels are identical

---

**Version**: 1.0  
**Last Updated**: May 19, 2026  
**Maintained by**: (Copilot reference)
