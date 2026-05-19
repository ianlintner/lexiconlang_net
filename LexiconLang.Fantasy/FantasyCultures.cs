using LexiconLang.Language;

namespace LexiconLang.Fantasy;

public static class FantasyCultures
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

    public static readonly Culture Dwarvish = new(
        Id: "fantasy.dwarvish",
        Glyphs: Archetypes.Guttural,
        MeaningPacks: new[] { CoreMeanings.Core, FantasyMeanings.Industrial },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "attribute"), (WordClass.Noun, "industry")),
            Surname: ComposeTemplate((WordClass.Noun, "earth"), (WordClass.Noun, "industry")),
            Settlement: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Noun, "structure"))),
        Capitalize: false);

    public static readonly Culture Elvish = new(
        Id: "fantasy.elvish",
        Glyphs: Archetypes.Flowing,
        MeaningPacks: new[] { CoreMeanings.Core, FantasyMeanings.Industrial },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "nature"), (WordClass.Noun, "nature")),
            Surname: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Noun, "nature"))),
        Capitalize: false);

    public static readonly Culture Orcish = new(
        Id: "fantasy.orcish",
        Glyphs: Archetypes.Guttural,
        MeaningPacks: new[] { CoreMeanings.Core, FantasyMeanings.Industrial },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "attribute"), (WordClass.Noun, "war")),
            Surname: ComposeTemplate((WordClass.Adjective, "attribute"), (WordClass.Noun, "war"))),
        Capitalize: false);

    public static readonly Culture Halfling = new(
        Id: "fantasy.halfling",
        Glyphs: Archetypes.Resonant,
        MeaningPacks: new[] { CoreMeanings.Core, FantasyMeanings.Industrial },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "attribute"), (WordClass.Noun, "nature")),
            Surname: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Noun, "society"))),
        Capitalize: false);

    public static readonly Culture Draconic = new(
        Id: "fantasy.draconic",
        Glyphs: Archetypes.Sibilant,
        MeaningPacks: new[] { CoreMeanings.Core, FantasyMeanings.Industrial },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "attribute"), (WordClass.Noun, "power"))),
        Capitalize: false);

    public static readonly Culture Plantoid = new(
        Id: "fantasy.plantoid",
        Glyphs: Archetypes.Flowing,
        MeaningPacks: new[] { CoreMeanings.Core, FantasyMeanings.Industrial },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "attribute"), (WordClass.Noun, "nature")),
            Surname: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Noun, "nature")),
            Settlement: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Noun, "structure"))),
        Capitalize: false);

    public static readonly Culture Mycanoid = new(
        Id: "fantasy.mycanoid",
        Glyphs: Archetypes.Sibilant,
        MeaningPacks: new[] { CoreMeanings.Core, FantasyMeanings.Industrial },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "attribute"), (WordClass.Noun, "nature")),
            Surname: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Noun, "nature")),
            Settlement: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Noun, "structure"))),
        Capitalize: false);

    public static readonly Culture Celestial = new(
        Id: "fantasy.celestial",
        Glyphs: Archetypes.Resonant,
        MeaningPacks: new[] { CoreMeanings.Core, FantasyMeanings.Industrial },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "divine"), (WordClass.Noun, "nature")),
            Surname: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Noun, "magic")),
            Settlement: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Noun, "structure"))),
        Capitalize: false);

    public static readonly Culture Fey = new(
        Id: "fantasy.fey",
        Glyphs: Archetypes.Flowing,
        MeaningPacks: new[] { CoreMeanings.Core, FantasyMeanings.Industrial },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "emotion"), (WordClass.Noun, "nature")),
            Surname: ComposeTemplate((WordClass.Noun, "nature"), (WordClass.Noun, "art"))),
        Capitalize: false);

    public static readonly Culture Tiefling = new(
        Id: "fantasy.tiefling",
        Glyphs: Archetypes.Sibilant,
        MeaningPacks: new[] { CoreMeanings.Core, FantasyMeanings.Industrial },
        Templates: new NameTemplates(
            Given: ComposeTemplate((WordClass.Adjective, "evil"), (WordClass.Noun, "power")),
            Surname: ComposeTemplate((WordClass.Noun, "magic"), (WordClass.Noun, "death"))),
        Capitalize: false);
}
