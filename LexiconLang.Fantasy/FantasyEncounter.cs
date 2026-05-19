using LexiconLang.Core;

namespace LexiconLang.Fantasy;

public sealed record FantasyEncounter(
    string Faction,
    string Leader,
    string Title,
    string Motive,
    string Description)
{
    public string Summary => $"{Leader} the {Title} leads the {Faction} to {Motive}. {Description}";
}

public static class FantasyEncounters
{
    public static readonly IGenerator<string, object?> FactionGenerator =
        Combinators.Compose<string, object?>("fantasy.faction", ctx =>
        {
            var prefix = Combinators.OneOf<string, object?>(FantasyData.PlacePrefixes).Generate(ctx.Child("prefix"));
            var type = Combinators.OneOf<string, object?>(FantasyData.FactionTypes).Generate(ctx.Child("type"));
            return $"The {prefix} {type}";
        });

    public static readonly IGenerator<string, object?> TitleGenerator =
        Combinators.Compose<string, object?>("fantasy.title", ctx =>
        {
            var adj = Combinators.OneOf<string, object?>(FantasyData.Adjectives["noble"].Concat(FantasyData.Adjectives["sinister"]).ToArray()).Generate(ctx.Child("adj"));
            var titleBase = Combinators.OneOf<string, object?>(FantasyData.Titles["noble"].Concat(FantasyData.Titles["martial"]).ToArray()).Generate(ctx.Child("base"));
            return $"{adj} {titleBase}";
        });

    public static readonly IGenerator<string, object?> MotiveGenerator =
        Combinators.Compose<string, object?>("fantasy.motive", ctx =>
        {
            var action = Combinators.OneOf<string, object?>(FantasyData.EpithetActions).Generate(ctx.Child("action")).ToLowerInvariant();
            var target = Combinators.OneOf<string, object?>(FantasyData.EpithetTargets).Generate(ctx.Child("target")).ToLowerInvariant();
            return $"{action} the {target}";
        });

    public static readonly IGenerator<string, object?> DescriptionGenerator =
        Combinators.Compose<string, object?>("fantasy.description", ctx =>
        {
            var opening = Combinators.OneOf<string, object?>(FantasyData.QuestHookOpenings).Generate(ctx.Child("op"));
            var subject = Combinators.OneOf<string, object?>(FantasyData.QuestHookSubjects).Generate(ctx.Child("sub"));
            var comp = Combinators.OneOf<string, object?>(FantasyData.QuestHookComplications).Generate(ctx.Child("comp"));
            return $"{opening} {subject} {comp}.";
        });

    public static FantasyEncounter MakeEncounter(Context<object?> ctx)
    {
        var culture = ctx.Rng.Next() > 0.5 ? FantasyCultures.Orcish : FantasyCultures.Elvish;
        var leader = LexiconLang.Language.Templates.GenerateName(culture, "given", ctx.Child("name"));

        var faction = FactionGenerator.Generate(ctx.Child("faction"));
        var title = TitleGenerator.Generate(ctx.Child("title"));
        var motive = MotiveGenerator.Generate(ctx.Child("motive"));
        var desc = DescriptionGenerator.Generate(ctx.Child("desc"));

        return new FantasyEncounter(faction, leader.Form, title, motive, desc);
    }
}