namespace LexiconLang.Language;

public enum WordClass
{
    Noun,
    Adjective,
    Verb,
    Particle,
}

public sealed record Meaning(string Id, WordClass Class, string[] Tags, string? Label = null);

public sealed record MeaningPack(string Id, string Version, Meaning[] Meanings);

public sealed record GlyphClass(string Id, string[] Glyphs);

public abstract record ConstraintRule
{
    public sealed record Forbid : ConstraintRule;
    public sealed record MaxOccurrences(int Max) : ConstraintRule;
}

public sealed record Constraint(string[] Pattern, ConstraintRule Rule);

public class GlyphSystem
{
    public GlyphClass[] Classes { get; init; } = Array.Empty<GlyphClass>();
    public string[][] Syllables { get; init; } = Array.Empty<string[]>();
    public string[] WordShapes { get; init; } = Array.Empty<string>();
    public Constraint[]? Constraints { get; init; }
    public string Joiner { get; init; } = "";

    public GlyphSystem() { }

    public GlyphSystem(
        GlyphClass[] classes,
        string[][] syllables,
        string[] wordShapes,
        Constraint[]? constraints = null,
        string joiner = "")
    {
        Classes = classes;
        Syllables = syllables;
        WordShapes = wordShapes;
        Constraints = constraints;
        Joiner = joiner;
    }
}

public sealed record NameTemplatePart(
    WordClass? Pick = null,
    string? Tag = null,
    bool Capitalize = false,
    string? Literal = null,
    string? Translation = null);

public enum NameTemplateKind { Compose, Literal }

public sealed record NameTemplate(
    NameTemplateKind Kind,
    NameTemplatePart[]? Parts = null,
    string? Sep = null,
    string? TransSep = null,
    string? Form = null,
    string? Translation = null);

public sealed record NameTemplates(
    NameTemplate? Given = null,
    NameTemplate? Surname = null,
    NameTemplate? Settlement = null,
    NameTemplate? Mountain = null,
    NameTemplate? River = null,
    NameTemplate? Forest = null);

public sealed record Culture(
    string Id,
    GlyphSystem Glyphs,
    MeaningPack[] MeaningPacks,
    NameTemplates Templates,
    bool Capitalize = false,
    object? VisualGlyphSystems = null);

public sealed record TranslatedName(
    string Form,
    string Translation,
    string Language,
    string[]? Parts = null,
    object? Glyphs = null)
{
    public override string ToString() => Form;
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lexiconlang_net
{
    public class Types
    {
        
    }
}