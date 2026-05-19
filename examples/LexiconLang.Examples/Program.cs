using System;

namespace LexiconLang.Examples;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("LexiconLang.Net Examples");
        Console.WriteLine("========================");
        
        RunExample("01: Quickstart", Example01_Quickstart.Run);
        RunExample("02: Batch Patrons", Example02_BatchPatrons.Run);
        RunExample("03: World Tree", Example03_WorldTree.Run);
        RunExample("04: Custom Generator", Example04_CustomGenerator.Run);
        RunExample("05: Custom Grammar", Example05_CustomGrammar.Run);
        RunExample("06: Custom Markov", Example06_CustomMarkov.Run);
        RunExample("07: Seed and Reroll", Example07_SeedAndReroll.Run);
        RunExample("08: Cross Genre", Example08_CrossGenre.Run);
        RunExample("09: Glyphs", Example09_Glyphs.Run);
    }

    private static void RunExample(string name, Action action)
    {
        Console.WriteLine($"\n--- {name} ---");
        action();
    }
}
