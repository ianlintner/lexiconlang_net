using LexiconLang.Language;

namespace LexiconLang.SciFi;

public static class SciFiCultures
{
    private static NameTemplate ComposeTemplate(
        (WordClass, string) pick1,
        (WordClass, string) pick2,
        string sep = "")
    {
        return new NameTemplate(
            NameTemplateKind.Compose,
            Parts: new[]
            {
                new NameTemplatePart(Pick: pick1.Item1, Tag: pick1.Item2, Capitalize: true),
                new NameTemplatePart(Pick: pick2.Item1, Tag: pick2.Item2),
            },
            Sep: sep);
    }

    public static readonly Culture Humanoid = new(
        Id: "scifi.humanoid",
        Glyphs: Archetypes.Resonant,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "technology"), (WordClass.Verb, "action"))),
        Capitalize: false);

    public static readonly Culture Insectoid = new(
        Id: "scifi.insectoid",
        Glyphs: Archetypes.Guttural,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "attribute"), (WordClass.Noun, "collective"))),
        Capitalize: false);

    public static readonly Culture Aquatic = new(
        Id: "scifi.aquatic",
        Glyphs: Archetypes.Flowing,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "biology"), (WordClass.Adjective, "biology"))),
        Capitalize: false);

    public static readonly Culture Synth = new(
        Id: "scifi.synth",
        Glyphs: new GlyphSystem(
            Archetypes.Clipped.Classes,
            Archetypes.Clipped.Syllables,
            Archetypes.Clipped.WordShapes,
            Archetypes.Clipped.Constraints,
            "-"),
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "technology"), (WordClass.Noun, "technology"))),
        Capitalize: false);
}
