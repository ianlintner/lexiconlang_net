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

    private static NameTemplate PickTemplate(WordClass wordClass, string tag)
    {
        return new NameTemplate(
            NameTemplateKind.Compose,
            Parts: new[]
            {
                new NameTemplatePart(Pick: wordClass, Tag: tag, Capitalize: true),
            },
            Sep: "");
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
            Given: ComposeTemplate((WordClass.Adjective, "collective"), (WordClass.Noun, "collective"))),
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
            Given: new NameTemplate(
                NameTemplateKind.Compose,
                Parts: new[]
                {
                    new NameTemplatePart(Pick: WordClass.Noun, Tag: "technology", Capitalize: true),
                    new NameTemplatePart(Literal: "0", Translation: "zero"),
                },
                Sep: "-")),
        Capitalize: false);

    public static readonly Culture BirdPeople = new(
        Id: "scifi.birdpeople",
        Glyphs: Archetypes.Flowing,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "flight"), (WordClass.Noun, "sound")),
            Surname: PickTemplate(WordClass.Adjective, "flight")),
        Capitalize: false);

    public static readonly Culture RockPeople = new(
        Id: "scifi.rockpeople",
        Glyphs: Archetypes.Guttural,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "geology"), (WordClass.Noun, "geology")),
            Surname: PickTemplate(WordClass.Adjective, "geology")),
        Capitalize: false);

    public static readonly Culture Mycoids = new(
        Id: "scifi.mycoids",
        Glyphs: Archetypes.Flowing,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "biology"), (WordClass.Verb, "biology")),
            Surname: PickTemplate(WordClass.Noun, "growth")),
        Capitalize: false);

    public static readonly Culture MammalianAlien = new(
        Id: "scifi.mammalian",
        Glyphs: Archetypes.Resonant,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Adjective, "nature")),
            Surname: PickTemplate(WordClass.Noun, "nature")),
        Capitalize: false);

    public static readonly Culture Plantoid = new(
        Id: "scifi.plantoid",
        Glyphs: Archetypes.Resonant,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Verb, "growth")),
            Surname: PickTemplate(WordClass.Noun, "nature")),
        Capitalize: false);

    public static readonly Culture Reptilian = new(
        Id: "scifi.reptilian",
        Glyphs: Archetypes.Sibilant,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "predator"), (WordClass.Adjective, "predator")),
            Surname: PickTemplate(WordClass.Noun, "predator")),
        Capitalize: false);

    public static readonly Culture Hivemind = new(
        Id: "scifi.hivemind",
        Glyphs: Archetypes.Clipped,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "network"), (WordClass.Noun, "hivemind"), ".")),
        Capitalize: false);

    public static readonly Culture Grayfolk = new(
        Id: "scifi.grayfolk",
        Glyphs: Archetypes.Resonant,
        MeaningPacks: new[] { CoreMeanings.Core, SciFiMeanings.Core },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Noun, "grayfolk"), (WordClass.Adjective, "grayfolk")),
            Surname: PickTemplate(WordClass.Noun, "knowledge")),
        Capitalize: false);
}
