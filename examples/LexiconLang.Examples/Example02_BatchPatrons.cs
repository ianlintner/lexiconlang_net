using System;
using System.Linq;
using LexiconLang.Core;
using LexiconLang.Fantasy;

namespace LexiconLang.Examples;

public static class Example02_BatchPatrons
{
    public static void Run()
    {
        var tavernSeed = LexiconContext.Create("the-gilded-anchor");

        var partyGen = Combinators.Repeat(FantasyEncounters.TitleGenerator, new CountSpec(Count: 6));
        var partyOfSix = partyGen.Generate(tavernSeed.Child("party"));

        Console.WriteLine("Adventuring party titles:");
        foreach (var member in partyOfSix)
        {
            Console.WriteLine($"  • {member}");
        }

        var patronsGen = Combinators.Repeat(FantasyEncounters.TitleGenerator, new CountSpec(Min: 5, Max: 12));
        var patrons = patronsGen.Generate(tavernSeed.Child("patrons"));

        Console.WriteLine($"\n{patrons.Count} patrons in the tavern tonight:");
        foreach (var p in patrons)
        {
            Console.WriteLine($"  • {p}");
        }
    }
}
