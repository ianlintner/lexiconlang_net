using System;
using LexiconLang.Core;

namespace LexiconLang.Examples;

public record IronKnight(string Name, string House, string Sword, string Motto, string Rank, int YearsOfService);

public static class Example04_CustomGenerator
{
    public static void Run()
    {
        var house = Combinators.OneOf<string, object?>("Vael", "Drachen", "Kessel", "Morain", "Strayle", "Vorden", "Halric", "Bayard");
        var sword = Combinators.OneOf<string, object?>("Ironwill", "Last Watch", "Quietsong", "Heartbreaker", "Verdict", "Six Kings", "Long Vigil", "Argent");
        var motto = Combinators.OneOf<string, object?>("Hold the line", "By steel and silence", "Fear the morning", "Strike first, repent later", "We are the second wall", "The crown waits");

        var rankParts = new[]
        {
            new Weighted<string>("Squire", 4),
            new Weighted<string>("Knight", 8),
            new Weighted<string>("Knight-Captain", 2),
            new Weighted<string>("Lord-Marshal", 1)
        };
        var rank = Combinators.WeightedList<string, object?>(rankParts, "ironknight.rank");

        var ironKnight = Combinators.Compose<IronKnight, object?>("ironknight", ctx =>
        {
            var r = rank.Generate(ctx.Child("rank"));
            return new IronKnight(
                "Sir " + house.Generate(ctx.Child("name")),
                house.Generate(ctx.Child("house")),
                sword.Generate(ctx.Child("sword")),
                motto.Generate(ctx.Child("motto")),
                r,
                ctx.Rng.NextInt(1, 40)
            );
        });

        var rootCtx = LexiconContext.Create("iron-watch");
        Console.WriteLine("The Iron Watch, current roster:\n");
        for (int i = 0; i < 5; i++)
        {
            var k = ironKnight.Generate(rootCtx.Child($"watch:{i}"));
            Console.WriteLine($"  {k.Rank.PadRight(15)} {k.Name.PadRight(20)} of House {k.House} — \"{k.Sword}\"");
        }
    }
}
