# LexiconLang TypeScript Feature Inventory

**Version**: Based on GitHub `ianlintner/lexiconlang` main branch (cloned May 19, 2026)  
**Purpose**: Comprehensive feature mapping to compare against the .NET port (`lexiconlang_net`)

---

## 1. Core Package (`@lexiconlang/core`)

### 1.1 RNG (Random Number Generation)

**Files**: `rng.ts`, `rng.test.ts`

**Type System**:

```typescript
type Seed = string | number | bigint | RngState;
interface RngState {
  algo: "sfc32";
  state: [number, number, number, number];
  origin: string;
}
```

**RNG Interface**:

- `next(): number` - Random float [0, 1)
- `nextU32(): number` - Unsigned 32-bit integer
- `nextInt(min, maxExclusive): number` - Random integer in range
- `nextRange(min, max): number` - Random float in range
- `pick<T>(items): T` - Pick random element from array
- `state(): RngState` - Serialize current state
- `fork(label): RNG` - Create child RNG with deterministic fork

**Algorithm**: SFC32 (Small Fast Chaotic)

- Seeded with `mix()` function for deterministic child RNG derivation
- Supports forking by label (string or number) for scoped randomness
- Origin tracking for debugging/replay

### 1.2 Context (`context.ts`)

**Type System**:

```typescript
interface Context<S = unknown> {
  rng: RNG;
  scope: readonly string[]; // Fork chain path
  tags: ReadonlySet<string>;
  locale?: string;
  data: S; // User data payload
  registry?: Registry;

  child(label, extra?): Context<S>;
  withTags(...tags): Context<S>;
  withData<S2>(data): Context<S2>;
}
```

**Key Features**:

- Immutable context model with `.child()` for fork chain
- Scope tracking for debugging
- Tag-based filtering/metadata
- Locale support for internationalization
- User data attachment (generic `S`)
- Registry attachment for generator lookup
- Options: `ContextOverrides`, `CreateContextOptions`

### 1.3 Generator System (`generator.ts`)

**Type System**:

```typescript
interface Generator<T = string, C = unknown> {
  id: string;
  generate(ctx: Context<C>): T;
}

type GeneratorLike = { id?: string; generate: (...args: any[]) => any };
```

**Utilities**:

- `asGenerator(fn)` - Wrap function as Generator
- `nextAnonId()` - Generate sequential IDs for unnamed generators

### 1.4 Combinators (`combinators.ts`)

**Primitive Generators**:

- `constant(value)` - Always return value
- `oneOf(...items)` - Uniform random pick
- `pickOf(weights)` - Weighted random pick
- `weightedList(items, options?)` - Weighted generator list
- `repeat(gen, count)` - Repeat generator N times (can be range)
- `intRange(min, max)` - Random integer in range
- `floatRange(min, max)` - Random float in range
- `compose(gen1, gen2, ...)` - Sequential composition
- `map(gen, fn)` - Transform generator output
- `chain(gen, fn)` - Monadic bind (fn returns Context => T)

**Type System**:

```typescript
type CountSpec = number | [min, max];
type WeightInput<T> =
  | T
  | readonly T[]
  | readonly { value: T; weight: number }[];
type ComposeOptions = { separator?: string; id?: string };
type GeneratorChoice<T> = T | Generator<T> | [T, weight];
```

### 1.5 Sampling & Alias Table (`sample.ts`)

**Type System**:

```typescript
interface Weighted<T> {
  value: T;
  weight: number;
}

interface AliasTable<T> {
  prob: readonly number[];
  alias: readonly (T | null)[];
}
```

**Functions**:

- `normalizeWeights(input)` - Parse `WeightInput` into `Weighted[]`
- `buildAliasTable(items)` - Build O(1) lookup table for weighted sampling
- `sampleAlias(table, rng)` - Pick from alias table in O(1)

**Algorithm**: Alias Method for efficient weighted random selection

### 1.6 Registry (`registry.ts`)

**Type System**:

```typescript
interface Registry {
  get(id: string): Generator<unknown> | undefined;
  getTyped<T>(id: string): Generator<T> | undefined;
  list(): Iterable<[string, Generator<unknown>]>;
}
```

**Functions**:

- `createRegistry()` - Create new Registry
- `RegistryImpl` class - Mutable registry implementation

### 1.7 Hashing & Utilities (`hash.ts`)

**Functions**:

- `fnv1a64(input: string): bigint` - FNV-1a 64-bit hash
- `splitmix64(x: bigint): bigint` - Splitmix64 hash function
- `mix(seed: bigint, label: string): bigint` - Mix seed with label string
- `seedToBigInt(seed)` - Convert Seed to bigint for RNG init

**Use**: Deterministic seed derivation for child RNG forks

---

## 2. Grammar Package (`@lexiconlang/grammar`)

### 2.1 Grammar System (`grammar.ts`)

**Type System**:

```typescript
type RuleValue<C = unknown> =
  | string
  | readonly string[]
  | WeightInput<string>
  | Generator<string, C>
  | (ctx: Context<C>) => string

type GrammarRules<C = unknown> = Record<string, RuleValue<C>>

interface GrammarOptions<C = unknown> {
  id?: string
  start?: string                           // Entry rule (default "start")
  modifiers?: Record<string, Modifier>     // Custom modifiers
  registry?: Registry
  maxDepth?: number                        // Recursion limit
}

interface Grammar<C = unknown> extends Generator<string, C> {
  id: string
  rules: GrammarRules<C>
  generate(ctx: Context<C>): string
  expand(template: string, ctx: Context<C>): string
}
```

**API**:

- `grammar(rules, options?)` - Create tracery-style grammar
- `expand(template, ctx)` - Expand single template string

**Features**:

- Tracery-style `#tag#` substitution syntax
- Nested rule expansion with tail recursion
- Arbitrary depth (respects maxDepth)
- Supports weighted rules, inline functions, generators
- Custom modifiers via pipes: `#rule | modifier1 | modifier2#`

### 2.2 Template System (`template.ts`)

**API**:

- `t` - Template tag function for `` t`string ${expr}` ``

### 2.3 Modifiers (`modifiers.ts`)

**Built-in Modifiers**:

- Text transformations: `capitalize`, `uppercase`, `lowercase`, `reverse`
- Selection: `a`, `s` (Tracery-style article selection)
- Conditional: `ifelse`, `switch`
- Custom modifier support via `Record<string, Modifier>`

**Type System**:

```typescript
interface Modifier {
  id: string;
  apply(value: string, ctx: Context): string;
}
```

### 2.4 Parser (`parser.ts`, `parser.test.ts`)

**API**:

- `parse(template: string)` - Parse template into AST

**AST Node Types**:

```typescript
type Node = string | ModifierCall | Action;

interface ModifierCall {
  type: "modifier";
  name: string;
  args: string[];
}

interface Action {
  type: "action";
  name: string;
  args: any[];
}
```

**Grammar**:

- Text nodes: `hello`
- Rule references: `#rule#`
- Weighted choices: `#[opt1:2, opt2:1]#`
- Modifiers: `#rule | modifier | modifier2#`
- Escaping: `\#` → `#`

---

## 3. Language Package (`@lexiconlang/language`)

### 3.1 Core Types (`types.ts`)

**Glyph System Types**:

```typescript
interface Constraint {
  pattern: readonly string[]; // Class names; "*" = wildcard
  rule: "forbid" | { maxOccurrences: number };
}

interface GlyphSystem {
  classes: Record<string, readonly string[]>; // Named glyph classes
  syllables: Array<[template: string, weight]>; // e.g. "C V", "C V C"
  wordShapes: Array<[shape: string, weight]>; // e.g. "1", "2", "1-2"
  constraints?: readonly Constraint[];
  joiner?: string; // Between syllables
}
```

**Lexicon Types**:

```typescript
type WordClass = "noun" | "adjective" | "verb" | "particle";

interface Meaning {
  id: string;
  class: WordClass;
  tags: readonly string[];
  label?: string;
}

interface MeaningPack {
  id: string;
  version: string;
  meanings: readonly Meaning[];
}

interface Lexicon {
  readonly cultureId: string;
  formOf(meaningId: string): string; // Cached word generation
  byClass(c: WordClass, tag?: string): readonly Meaning[];
  materialize(): ReadonlyMap<string, string>;
}

interface Culture {
  id: string;
  glyphs: GlyphSystem;
  meaningPacks: MeaningPack[];
  templates: NameTemplate[];
  conjugations?: Record<string, Grammar>;
}
```

**Name/Translation Types**:

```typescript
interface TranslatedName {
  form: string; // Conlang word
  translation: string; // Back-translation/glossing
  details?: Record<string, any>;
}

interface NameTemplate {
  id: string;
  class: WordClass;
  pattern: TemplatePart[];
  weight?: number;
  tags?: readonly string[];
}

type TemplatePart =
  | { type: "literal"; text: string }
  | { type: "meaning"; meaningClass: WordClass; tags?: string[] };
```

**Visual Glyph Types**:

```typescript
type RenderFormat = "svg" | "unicode" | "canvas";
type MappingStrategy = "phoneme" | "morpheme" | "holistic";

interface Glyph {
  id: string;
  meaning?: string;
  svg?: string;
  canvasInstructions?: Array<{ type: string; params: Array<number | string> }>;
  unicode?: string;
}

interface GlyphSet {
  phonetic?: Glyph[]; // One per 2-char unit (phoneme strategy)
  conceptual?: Glyph[]; // One per morpheme (morpheme strategy)
  holistic?: Glyph; // Single glyph (holistic strategy)
}

interface VisualGlyphSystem {
  id: string;
  type: "alphabet" | "conceptual";
  renderFormat: RenderFormat;
  mappingStrategy: MappingStrategy;
  generator?: {
    baseShapes: Array<"rect" | "circle" | "line" | "arc" | "polygon">;
    complexity: "simple" | "medium" | "complex";
    symmetry: boolean;
    palette?: string[];
  };
  templates?: Array<{
    id: string;
    baseShape: "rect" | "circle" | "line";
    variants: number;
    modifiers: Array<"rotate" | "scale" | "stroke">;
  }>;
  unicodeMappings?: Record<string, string>;
  renderParams?: {
    size?: number;
    strokeWidth?: number;
    fallback?: string;
  };
}
```

### 3.2 Phonotactics (`phonotactics.ts`)

**API**:

- `generateWord(glyphSystem, ctx): string` - Generate word conforming to phonotactic rules

**Algorithm**:

1. Pick word shape (syllable count) from weighted list
2. For each syllable, pick syllable template
3. For each class slot, pick a glyph from that class
4. Check constraints; retry glyph if violated (up to 8 retries)
5. Join syllables with optional joiner
6. Capitalize

**Constraint Checking**:

- Pattern matching on class sequences
- "forbid" rule: never allow this sequence
- `{ maxOccurrences }`: limit consecutive repeats

### 3.3 Lexicon Building (`lexicon.ts`)

**API**:

- `buildLexicon(culture, ctx): Lexicon` - Create deterministic, key-addressed lexicon

**Features**:

- Order-independent word generation (key-addressed by meaning ID)
- Fork semantics: each meaning ID gets own RNG fork
- Lazy evaluation with caching
- Materialization for bulk export

### 3.4 Meanings & Archetypes (`meanings.ts`, `archetypes.ts`)

**Provided Packs**:

- `coreMeanings` - Base semantic inventory

**API**:

- `byClass(wordClass, tag?)` - Query meanings by grammatical class and tag

### 3.5 Name Generation & Templates (`templates.ts`)

**API**:

- `generateName(culture, templateClass, ctx): TranslatedName` - Generate named entity
- Returns form + translation/glossing + details

### 3.6 Glyph Constraints (`glyphs.ts`)

**API**:

- `constraintMatches(pattern, classSequence): boolean` - Check if pattern matches

---

## 4. Markov Package (`@lexiconlang/markov`)

### 4.1 Model Type (`model.ts`)

**Type System**:

```typescript
interface MarkovModel {
  order: number
  minLength: number
  maxLength: number
  transitions: Record<string, Record<string, number>>
  forbidden?: readonly string[]
  meta?: Record<string, unknown>
}

interface MarkovModelJSON extends MarkovModel {
  // JSON-serializable form
}

const START = "\0"    // Start of word sentinel
const END = "\1"      // End of word sentinel
```

**Functions**:

- `toJSON(model)` - Serialize to JSON
- `fromJSON(json)` - Deserialize from JSON

### 4.2 Trainer (`trainer.ts`)

**Type System**:

```typescript
interface TrainOptions {
  order?: number; // n-gram order (default 3)
  minLength?: number; // Min generated length (default 3)
  maxLength?: number; // Max generated length (default 14)
  rejectSubstringsOfLength?: number; // Forbid substrings >= length
  pruneBelow?: number; // Min transition count (default 1)
  lowercase?: boolean; // Lowercase corpus (default true)
  meta?: Record<string, unknown>;
}

type Corpus = readonly string[] | readonly { word: string; weight?: number }[];
```

**API**:

- `train(corpus, options?): MarkovModel` - Train Markov model from corpus

**Algorithm**:

- N-gram order up to `order` parameter
- Weighted entries supported
- Substring rejection: prevents memorizing training data
- Pruning: removes rare transitions
- START/END sentinels mark word boundaries

### 4.3 Sampler (`sampler.ts`)

**Type System**:

```typescript
interface SampleOptions {
  maxAttempts?: number; // Retry limit (default 200)
  minLength?: number; // Override model minLength
  maxLength?: number; // Override model maxLength
  capitalize?: boolean; // Capitalize first letter (default true)
}

interface MarkovGeneratorOptions {
  id?: string;
}
```

**API**:

- `sample(model, rng, options?): string` - Sample single word
- `markov(model, options?): Generator<string>` - Create generator

**Algorithm**:

- Weighted context selection based on transitions
- Context backoff: if context not found, try shorter context
- Retry logic: redraw on failure up to maxAttempts
- Length enforcement: respects min/max length constraints

---

## 5. Glyphs Package (`@lexiconlang/glyphs`)

### 5.1 Glyph Generation (`glyphs.ts`)

**API**:

- `glyphsFor(name: TranslatedName, system: VisualGlyphSystem, ctx: Context): GlyphSet`

**Mapping Strategies**:

- **phoneme**: Split form into 2-character units → one glyph per unit
- **morpheme**: Split translation by "-" → one glyph per morpheme
- **holistic**: Single glyph for entire name

### 5.2 Shape Generation (`shape-generator.ts`)

**API**:

- `generateShapes(ctx: Context): string[]` - Generate procedural base shapes

**Features**:

- Base shapes: rect, circle, line, arc, polygon
- Complexity levels: simple, medium, complex
- Symmetry support
- Palette customization

### 5.3 Renderers

#### SVG Renderer (`svg-renderer.ts`)

- **API**: `renderToSVG(glyph, params): string`
- Output: SVG string

#### Unicode Renderer (`unicode-renderer.ts`)

- **API**: `renderToUnicode(glyph, config): string`
- Output: Unicode character or fallback
- `UnicodeRegistry` - Character-to-glyph mapping
- `UnicodeConfig` interface

#### Canvas Renderer (`canvas-renderer.ts`)

- **API**: `renderToCanvas(glyph, params): CanvasInstruction[]`
- Output: Canvas drawing instructions
- `executeCanvasInstructions(ctx, instructions)` - Execute on canvas

**Instruction Types**:

```typescript
type CanvasInstruction = {
  type: string;
  params: Array<number | string>;
};
```

### 5.4 Types Export (`types.ts`)

Re-exports from `@lexiconlang/language`:

- Glyph, GlyphSet, VisualGlyphSystem, RenderFormat, MappingStrategy, TranslatedName

Glyphs-specific types:

- BaseShape, Complexity, ShapeParams, CanvasInstruction, RenderParams

---

## 6. Content Packs

### 6.1 Fantasy Pack (`@lexiconlang/fantasy`)

**Source Files**: `corpora.ts`, `data.ts`, `language/cultures.ts`, `language/meanings.ts`

**Cultures**: 10 predefined races

1. **Dwarvish** - Sturdy consonant-heavy phonology
2. **Elvish** - Melodic vowel-rich phonology
3. **Orcish** - Guttural, aggressive phonology
4. **Halfling** - Diminutive playful phonology
5. **Draconic** - Dragon language (sibilant-heavy)
6. **Plantoid** - Plant-derived creatures
7. **Mycanoid** - Fungal creatures
8. **Celestial** - Holy, ethereal beings
9. **Fey** - Magical forest creatures
10. **Tiefling** - Demonic beings

**Data Packs** (in `data.ts`):

- adjectives, animals, armorPieces, dragonAdjectives, dragonColors
- epithetActions, epithetTargets, factionTypes, occupations, personalityTraits
- placePrefixes, placeSuffixes, questHookComplications, questHookOpenings, questHookSubjects
- tavernNouns, titles, weapons

**Corpora** (in `corpora.ts`):

- Predefined lists for Markov training: `elven_male`, `elven_female`, `dwarven_male`, `dwarven_female`, `human_male`, `human_female`, `orcish`

**Exports**:

- Generators:
  - `markovElvenMale`, `markovElvenFemale`, etc. (deprecated Markov-based)
  - Culture objects for lexicon-based generation
- Data lists for NPCs, encounters, etc.
- `makeFantasyEncounter(ctx)` - Complex generator

### 6.2 SciFi Pack (`@lexiconlang/scifi`)

**Cultures**: 12 alien species

1. **Humanoid** - Generic alien humanoids
2. **Insectoid** - Bug-like creatures
3. **Aquatic** - Water-dwelling species
4. **Synth** - Android/robotic
5. **Birdpeople** - Avian species
6. **Rockpeople** - Mineral-based life
7. **Mycoid** - Fungal/spore-based
8. **Mammalian** - Mammal-like aliens
9. **Plantoid** - Plant-based entities
10. **Reptilian** - Reptile-like species
11. **Hivemind** - Collective consciousness
12. **Grayfolk** - Stereotypical "gray aliens"

**Exports**:

- Generators (language-based):
  - `humanoidName`, `insectoidName`, `aquaticName`, `synthName`, etc.
- Deprecated Markov generators (marked for removal in v0.3)

### 6.3 Modern Pack (`@lexiconlang/modern`)

**Generators**:

- `givenMaleName`, `givenFemaleName`, `surname` - Real-world names
- `sex` - Binary sex choice
- `personName` - Full person name (given + surname + sex)
- `cityName` - Grammar-based modern city names
- `streetName` - Grammar-based street names
- `companyName` - Grammar-based company/business names

**Data**:

- Given names (male/female): ~30-35 diverse names each
- Surnames: ~50 surnames with global diversity
- City components: prefixes, suffixes, proper nouns
- Street types, ordinals, tree types
- Company name components and suffixes

---

## 7. CLI Package (`@lexiconlang/cli`)

### 7.1 Command Structure

**Entry Point**: `index.ts` (shebang `#!/usr/bin/env node`)

**Commands**:

#### `build-markov <input.json> --out <model.json>`

Train a Markov model from corpus JSON

- Input formats: `string[]` or `{ word, weight }[]`
- Options:
  - `--order <n>` - Markov order (default 3)
  - `--min-length <n>` - Min length (default 3)
  - `--max-length <n>` - Max length (default 14)
  - `--reject-substrings-of-length <n>` - Prevent memorization
  - `--no-lowercase` - Keep original case
- Implementation: `commands/build-markov.ts`

#### `scaffold-pack <name> [--dir <packages>]`

Create new content pack skeleton

- Generates project structure for new content pack
- Default directory: `packages/<name>`
- Implementation: `commands/scaffold-pack.ts`

#### `help, --help, -h`

Show usage information

### 7.2 Args Parser (`args.ts`, `args.test.ts`)

**API**:

- `parseArgs(argv)` - Parse command-line arguments

**Format**:

```typescript
interface ParsedArgs {
  command: string | null;
  options: Record<string, string | boolean | null>;
}
```

---

## 8. Testing & Examples

### 8.1 Tests

**Test Files by Package**:

- `core/`: `rng.test.ts`, `combinators.test.ts`
- `grammar/`: `grammar.test.ts`, `parser.test.ts`
- `language/`: `__tests__/{determinism,lexicon,meanings,phonotactics,templates}.test.ts`
- `markov/`: `markov.test.ts`
- `glyphs/`: `glyphs.test.ts`, `exports.test.ts`
- `fantasy/`: `fantasy.test.ts`, `language/cultures.test.ts`
- `scifi/`: `scifi.test.ts`, `language/cultures.test.ts`
- `modern/`: `modern.test.ts`
- `cli/`: `args.test.ts`

**Test Framework**: Vitest

**Test Coverage Areas**:

- Determinism verification (same seed → same output)
- Constraint satisfaction (phonotactics)
- Markov training correctness
- Parser correctness
- Export validation

### 8.2 Examples

**Located**: `examples/` directory

1. **01-quickstart.ts** - Basic grammar + context
2. **02-batch-patrons.ts** - Batch name generation
3. **03-world-tree.ts** - Hierarchical context forking
4. **04-custom-generator.ts** - Implement custom Generator
5. **05-custom-grammar.ts** - Custom grammar rules
6. **06-custom-markov.ts** - Train + sample Markov
7. **07-seed-and-reroll.ts** - Determinism demo
8. **08-cross-genre.ts** - Multiple packs in single run
9. **09-glyphs.ts** - Glyph generation + rendering

**Runner**: `_run-all.ts` - Execute all examples

---

## 9. Advanced Features & Algorithms

### 9.1 Determinism & Reproducibility

**Key Mechanisms**:

- All randomness flows through `Context.rng`
- Fork semantics via labeled child contexts: `ctx.child("label")`
- Seed-derived child RNG using deterministic hash (mix + FNV-1a + SplitMix64)
- Lexicon: key-addressed by meaning ID → order-independent
- Markov: START/END sentinels for consistent boundaries

**Guarantees**:

- Same seed + same code path = same output
- Reordering independent operations (different forks) doesn't change results
- Determinism supported across semantic versioning

### 9.2 Composition Patterns

**Available**:

- `compose(gen1, gen2, ...)` - Sequential composition with optional separator
- `map(gen, fn)` - Functor mapping
- `chain(gen, fn)` - Monadic bind
- `repeat(gen, count)` - Repetition (fixed or range)

**Use Cases**:

- Building complex generators from simpler ones
- Pipeline processing of content
- Context-aware transformations

### 9.3 Constraint System

**Phonotactic Constraints**:

- Pattern matching on syllable class sequences
- "forbid" rule: explicitly disallow
- `{ maxOccurrences }` rule: limit consecutive glyphs
- Retry-based satisfaction (up to 8 retries per violation)

### 9.4 Fork Label Semantics

**Purpose**: Deterministic child RNG derivation

**Example**:

```
seed: "test"
  ↓ fork("name")
  → child RNG used for names
  ↓ fork("age")
  → child RNG used for ages
```

**Result**: Changing order doesn't affect other branches

### 9.5 Markov Substring Rejection

**Feature**: Prevent memorization of training data

**Mechanism**:

- Track all substrings of corpus entries up to length N
- Reject any generated string containing those substrings
- Prevents unintended plagiarism in training-heavy domains

**Trade-off**: More rejects on smaller corpora; safeguard against overfitting

---

## 10. Data & Semantic Content

### 10.1 Meaning System

**Core Meanings** (from `coreMeanings`):

- Semantic inventory organized by WordClass
- Tags for filtering (e.g., "combat", "nature", "magic")
- Stable IDs for content pack versioning

### 10.2 Corpora

**Location**: `fantasy/src/corpora.ts`

**Content**:

- Predefined name lists organized by race + gender
- Trained Markov models for name generation
- Weighted corpus support

### 10.3 Culture Definitions

**Stored**: Content pack `language/cultures.ts` files

**Structure**:

- GlyphSystem: phoneme classes + syllable/word templates + constraints
- MeaningPacks: semantic inventory
- NameTemplates: how to construct names from meanings
- Optional conjugations: Grammar rules for word mutation

---

## 11. Package.json & Distribution

**Monorepo Structure**:

```
lexiconlang_ts/
├── package.json (root, workspaces)
├── packages/
│   ├── core/
│   ├── grammar/
│   ├── language/
│   ├── markov/
│   ├── glyphs/
│   ├── fantasy/
│   ├── scifi/
│   ├── modern/
│   └── cli/
├── examples/
├── tests/
├── docs-site/
└── vitest.config.ts
```

**Publishing**:

- Each package has `package.json` with metadata
- Library packages (core, grammar, etc.) are published to npm
- CLI is installed as global tool via `npm i -g @lexiconlang/cli`
- Test/example packages not published

---

## 12. Build & Tooling

**TypeScript Configuration**:

- Root `tsconfig.json` with `"composite": true`
- Each package has local `tsconfig.json`
- Compiler options: ESNext target, Node.js module resolution

**Testing**:

- Vitest configuration: `vitest.config.ts`
- Test patterns: `*.test.ts`, `__tests__/*.test.ts`
- Determinism tests critical

**Documentation**:

- VitePress docs site: `docs-site/`
- Config: `docs-site/.vitepress/config.ts`

---

## 13. Export Summary by Module

| Module       | Key Exports                                                                                                                                                                                 |
| ------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **core**     | `Seed, RNG, Sfc32, Context, createContext, Generator, Registry, createRegistry, weightedList, oneOf, compose, map, chain, buildAliasTable, sampleAlias`                                     |
| **grammar**  | `grammar, Grammar, GrammarRules, t, builtinModifiers, Modifier, parse, Node`                                                                                                                |
| **language** | `GlyphSystem, Meaning, Culture, Lexicon, buildLexicon, generateWord, Constraint, MeaningPack, TranslatedName, NameTemplate, TemplatePart, generateName, Glyph, GlyphSet, VisualGlyphSystem` |
| **markov**   | `MarkovModel, MarkovModelJSON, train, TrainOptions, sample, markov, SampleOptions`                                                                                                          |
| **glyphs**   | `generateShapes, renderToSVG, renderToUnicode, renderToCanvas, glyphsFor, Glyph, GlyphSet, VisualGlyphSystem, UnicodeRegistry`                                                              |
| **fantasy**  | 10 Culture objects (dwarvish, elvish, ...), data lists, Markov generators, `makeFantasyEncounter`                                                                                           |
| **scifi**    | 12 Culture objects (humanoid, insectoid, ...), name generators                                                                                                                              |
| **modern**   | `personName, cityName, streetName, companyName, givenMaleName, givenFemaleName, surname`                                                                                                    |

---

## 14. Comparison Reference for .NET Port

### Feature Checklist

- [ ] RNG: SFC32 with forking semantics
- [ ] Context: Immutable context with scope/tags/locale
- [ ] Generator trait/interface: Standardized generation signature
- [ ] Combinators: oneOf, pickOf, repeat, compose, map, chain
- [ ] Alias table: Weighted sampling
- [ ] Grammar: Tracery-style with modifiers and pipes
- [ ] Parser: Template → AST
- [ ] Phonotactics: Syllable + word shape + constraints
- [ ] Lexicon: Key-addressed, order-independent
- [ ] Markov: Train + sample with substring rejection
- [ ] Glyphs: Generation + 3 rendering strategies (SVG, Unicode, Canvas)
- [ ] Content packs: Fantasy (10), SciFi (12), Modern (4 name types)
- [ ] CLI: build-markov, scaffold-pack
- [ ] Determinism: Verified across operations
- [ ] Tests: Comprehensive, deterministic

### Known Differences in .NET Port (lexiconlang_net)

- Language: C# instead of TypeScript
- Platform: .NET 8.0 instead of Node.js
- File-scoped namespaces vs ES modules
- Records vs interfaces for data types (preferred in .NET)
- No async/await (synchronous generation)
- MSBuild + NuGet instead of npm + ESM

---

## 15. Version & Build Info

**TypeScript Repo**:

- **GitHub**: `ianlintner/lexiconlang` (main branch)
- **Last cloned**: May 19, 2026
- **Node.js**: ESM modules, TypeScript 5.x
- **Package Manager**: npm or yarn workspaces

**Key Files**:

- Root: `package.json`, `tsconfig.json`, `vitest.config.ts`
- Monorepo workspaces defined in root `package.json`

---

**End of Inventory**

This document serves as the canonical feature reference for comparing the TypeScript implementation against the .NET `lexiconlang_net` port. Use sections 1–10 to verify API coverage, and section 14 as a checklist for completeness.
