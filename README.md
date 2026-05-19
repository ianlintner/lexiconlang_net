# LexiconLang.Net

A complete C#/.NET port of the deterministic procedural generation system [LexiconLang](https://github.com/ianlintner/lexiconlang). Designed for games, tabletop tools, and narratives, focusing on absolute determinism, composability, and 1-to-1 parity with the TypeScript library.

## Features

- **Absolute Determinism**: The entire generation tree is driven by a hierarchical `Context` seeded randomly or strictly. `Context.Child("node")` automatically forks the Random Number Generator to maintain isolated state streams—meaning adding or removing content elsewhere won't shift unrelated lists!
- **Data-Driven & Composable**: Use small atomic string/object generators combined via `Combinators.Repeat`, `Compose`, `WeightedList`, etc.
- **Tracery-style Grammars**: `LexiconLang.Grammar` supports complex, recursively expanded templates and custom text modifiers (`#name.capitalize#`).
- **Markov Models**: `LexiconLang.Markov` allows you to train text models to procedurally generate cohesive names and prose.
- **Phonotactics & Glyphs**: `LexiconLang.Language` and `LexiconLang.Glyphs` enable generation of culturally distinct languages, unique words, and corresponding SVG procedural runes/glyphs.
- **Content Packs out-of-the-box**: `LexiconLang.Fantasy`, `LexiconLang.SciFi`, and `LexiconLang.Modern`.

## Included Packages

- `LexiconLang.Core` (RNG, `Context`, Combinators)
- `LexiconLang.Grammar`
- `LexiconLang.Markov`
- `LexiconLang.Language`
- `LexiconLang.Glyphs`
- `LexiconLang.Fantasy`, `LexiconLang.SciFi`, `LexiconLang.Modern`

## Usage Examples

### 1. Minimal Quickstart

```csharp
using System;
using LexiconLang.Core;
using LexiconLang.Fantasy;

class Program
{
    static void Main()
    {
        // 1. Initialize root context with a deterministic seed
        var ctx = LexiconContext.Create("hello-world");

        // 2. Generate random results from an included content pack
        var faction = FantasyEncounters.FactionGenerator.Generate(ctx.Child("faction"));
        var title = FantasyEncounters.TitleGenerator.Generate(ctx.Child("title"));

        Console.WriteLine($"The {title} of {faction}");
    }
}
```

### 2. Custom Generators

```csharp
public record IronKnight(string Name, string Rank);

var house = Combinators.OneOf<string, object?>("Vael", "Kessel", "Vorden", "Bayard");

var rank = Combinators.WeightedList<string, object?>(new[]
{
    new Weighted<string>("Squire", 4),
    new Weighted<string>("Knight", 8),
    new Weighted<string>("Knight-Captain", 2),
});

var ironKnight = Combinators.Compose<IronKnight, object?>("ironknight", ctx =>
{
    return new IronKnight(
        "Sir " + house.Generate(ctx.Child("name")),
        rank.Generate(ctx.Child("rank"))
    );
});

var rootCtx = LexiconContext.Create("iron-watch");
var k = ironKnight.Generate(rootCtx.Child("watch:0"));
```

### 3. Grammars (Tracery Style)

```csharp
var rules = new GrammarRules<object?>
{
    { "start", new Weighted<string>[]
        {
            new ("#prefix.capitalize# #element.capitalize# #form.capitalize#", 5),
            new ("the #adj.capitalize# #form.capitalize#", 2)
        }
    },
    { "prefix", new[] { "lesser", "greater", "true" } },
    { "element", new[] { "fire", "frost", "shadow" } },
    { "form", new[] { "bolt", "ward", "veil" } },
    { "adj", new[] { "unsleeping", "patient", "errant" } }
};

var spellName = new Grammar<object?>(rules);
var ctx = LexiconContext.Create("spellbook");

// Prints e.g. "Greater Shadow Bolt"
Console.WriteLine(spellName.Generate(ctx)); 
```

### 4. Markov Model

```csharp
var corpus = new[] { "caernarfon", "carmarthen", "ceredigion", "conwy", "gwynedd" };
var entries = corpus.Select(x => new TrainEntry(x));

// Train the generator
var model = Trainer.Train(entries, new TrainOptions(Order: 3, MinLength: 5, MaxLength: 12));
var townName = Sampler.AsGenerator<object?>(model);

var ctx = LexiconContext.Create("welsh-towns");
Console.WriteLine(townName.Generate(ctx));
```

### 5. Culture & Glyphs

```csharp
var ctx = LexiconContext.Create("glyph-demo");

// Generate a culturally appropriate name and translation
var dwarfName = Templates.GenerateName(FantasyCultures.Dwarvish, "given", ctx.Child("dwarf"));

// Setup the glyph system to be phoneme-based
var visualSystem = new VisualGlyphSystem(
    Name: "dwarvish-runes",
    Strategy: MappingStrategy.Phoneme,
    Complexity: Complexity.Medium,
    Format: RenderFormat.Svg,
    Palette: new[] { "#333", "#666" }
);

// Map the generated name strictly structure-to-glyph
var dwarfGlyphs = GlyphGenerator.GlyphsFor(dwarfName, visualSystem, ctx.Child("dwarf"));
```

## Build and test

```bash
dotnet build -warnaserror
dotnet test LexiconLang.Net.slnx
```
