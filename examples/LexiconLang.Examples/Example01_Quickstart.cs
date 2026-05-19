using System;
using LexiconLang.Core;
using LexiconLang.Fantasy;

namespace LexiconLang.Examples;

public static class Example01_Quickstart
{
    public static void Run()
    {
        var ctx = LexiconContext.Create("hello-world");

        Console.WriteLine("A random Encounter:");
        var faction = FantasyEncounters.FactionGenerator.Generate(ctx.Child("faction"));
        var title = FantasyEncounters.TitleGenerator.Generate(ctx.Child("title"));
        Console.WriteLine($"  The {title} of {faction}");
    }
}
