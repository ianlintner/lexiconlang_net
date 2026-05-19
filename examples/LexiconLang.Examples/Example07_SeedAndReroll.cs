using System;
using System.Collections.Generic;
using LexiconLang.Core;
using LexiconLang.Fantasy;

namespace LexiconLang.Examples;

public static class Example07_SeedAndReroll
{
    public static void Run()
    {
        Console.WriteLine("Simulating saving and rerolling exact subsets.\n");
        var saveRerolls = new Dictionary<string, int>();

        string GetSettlement(string path)
        {
            saveRerolls.TryGetValue(path, out var v);
            var ctx = LexiconContext.Create("shared-realm").Child(path).Child($"v:{v}");
            return FantasyEncounters.TitleGenerator.Generate(ctx);
        }

        Console.WriteLine("Three titles initially:");
        for (int i = 0; i < 3; i++)
            Console.WriteLine($"  Title {i}  →  {GetSettlement($"title:{i}")}");

        Console.WriteLine("\nPlayer rerolls Title 1:");
        saveRerolls["title:1"] = 1;

        for (int i = 0; i < 3; i++)
        {
            var note = i == 1 ? "  ← REROLLED" : "";
            Console.WriteLine($"  Title {i}  →  {GetSettlement($"title:{i}")}{note}");
        }
    }
}
