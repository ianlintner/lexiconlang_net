using System;
using LexiconLang.Core;
using LexiconLang.Fantasy;
using LexiconLang.Language;

namespace LexiconLang.Examples;

public static class Example03_WorldTree
{
    public static void Run()
    {
        var world = LexiconContext.Create("campaign-of-iron");

        string[] regionNames = { "Northmarch", "Sunken Coast", "Ember Reach" };
        var ruler = FantasyEncounters.TitleGenerator.Generate(world.Child("ruling-faction"));

        Console.WriteLine($"World: ruled by {ruler}");
        Console.WriteLine(new string('-', 60));

        for (int r = 0; r < regionNames.Length; r++)
        {
            var region = world.Child($"region:{r}");
            var capital = Templates.GenerateName(FantasyCultures.Dwarvish, "given", region.Child("capital"));
            
            Console.WriteLine($"\n{regionNames[r]} — capital: {capital.Form}");

            var settlements = Combinators.Repeat(FantasyEncounters.TitleGenerator, new CountSpec(Count: 2)).Generate(region.Child("settlements"));
            foreach (var s in settlements)
            {
                Console.WriteLine($"  {s}");
            }
        }
    }
}
