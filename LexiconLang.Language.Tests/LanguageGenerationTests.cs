using LexiconLang.Core;
using LexiconLang.Language;

namespace LexiconLang.Language.Tests;

public class LanguageGenerationTests
{
    [Fact]
    public void GlyphClassContainsGlyphs()
    {
        var glyphClass = new GlyphClass("vowels", new[] { "a", "e", "i", "o", "u" });

        Assert.Equal("vowels", glyphClass.Id);
        Assert.Equal(5, glyphClass.Glyphs.Length);
    }

    [Fact]
    public void MeaningPackContainsMeanings()
    {
        var meanings = new[]
        {
            new Meaning("m1", WordClass.Noun, Array.Empty<string>()),
            new Meaning("m2", WordClass.Verb, Array.Empty<string>())
        };

        var pack = new MeaningPack("test", "1.0", meanings);

        Assert.Equal("test", pack.Id);
        Assert.Equal("1.0", pack.Version);
        Assert.Equal(2, pack.Meanings.Length);
    }

    [Fact]
    public void CultureDataIsMaintained()
    {
        var glyphSystem = new GlyphSystem();
        var meanings = new[] { new Meaning("test", WordClass.Noun, Array.Empty<string>()) };
        var meaningPack = new MeaningPack("pack", "1.0", meanings);

        var culture = new Culture(
            "myculture",
            glyphSystem,
            new[] { meaningPack },
            new NameTemplates());

        Assert.Equal("myculture", culture.Id);
        Assert.NotNull(culture.Glyphs);
    }

    [Fact]
    public void ConstraintRuleTypes()
    {
        var forbid = new ConstraintRule.Forbid();
        Assert.NotNull(forbid);

        var maxOcc = new ConstraintRule.MaxOccurrences(5);
        Assert.Equal(5, maxOcc.Max);
    }

    [Fact]
    public void ConstraintCanBeCreated()
    {
        var forbid = new ConstraintRule.Forbid();
        var constraint = new Constraint(new[] { "aa", "ee" }, forbid);

        Assert.Equal(2, constraint.Pattern.Length);
        Assert.IsType<ConstraintRule.Forbid>(constraint.Rule);
    }

    [Fact]
    public void NameTemplatePartCanBeLiteral()
    {
        var part = new NameTemplatePart(Literal: "Sir");

        Assert.Equal("Sir", part.Literal);
        Assert.Null(part.Pick);
    }

    [Fact]
    public void NameTemplatesCanHaveTemplates()
    {
        var templates = new NameTemplates(
            Given: null,
            Surname: null,
            Settlement: null);

        Assert.Null(templates.Given);
        Assert.Null(templates.Surname);
    }

    [Fact]
    public void TranslatedNameHasTranslation()
    {
        var translatedName = new TranslatedName(
            Form: "Aelindor",
            Translation: "elf-lord",
            Language: "elvish");

        Assert.Equal("Aelindor", translatedName.Form);
        Assert.Equal("elf-lord", translatedName.Translation);
        Assert.Equal("elvish", translatedName.Language);
    }

    [Fact]
    public void WordClassEnumHasValues()
    {
        var classes = Enum.GetValues<WordClass>();
        Assert.True(classes.Length > 0);
        Assert.Contains(WordClass.Noun, classes);
        Assert.Contains(WordClass.Verb, classes);
    }

    [Fact]
    public void NameTemplateKindEnumHasValues()
    {
        var kinds = Enum.GetValues<NameTemplateKind>();
        Assert.True(kinds.Length > 0);
        Assert.Contains(NameTemplateKind.Literal, kinds);
    }

    [Fact]
    public void GlyphSystemCanBeInitializedEmpty()
    {
        var glyphSystem = new GlyphSystem();
        Assert.NotNull(glyphSystem.Classes);
        Assert.NotNull(glyphSystem.Syllables);
        Assert.NotNull(glyphSystem.WordShapes);
    }

    [Fact]
    public void GlyphSystemCanBeInitializedWithData()
    {
        var classes = new[] { new GlyphClass("vowel", new[] { "a", "e" }) };
        var syllables = new string[][] { new string[] { "v" } };
        var wordShapes = new[] { "v" };

        var glyphSystem = new GlyphSystem(classes, syllables, wordShapes);

        Assert.Single(glyphSystem.Classes);
        Assert.Single(glyphSystem.Syllables);
        Assert.Single(glyphSystem.WordShapes);
    }
}
