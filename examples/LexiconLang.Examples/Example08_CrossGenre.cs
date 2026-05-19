using System;
using LexiconLang.Core;
using LexiconLang.Modern;

namespace LexiconLang.Examples;

public static class Example08_CrossGenre
{
    public static void Run()
    {
        var ctx = LexiconContext.Create("the-ironwake");
        Console.WriteLine("Cross-Genre Generation:\n");
        
        var name = LexiconLang.Modern.ModernNames.StreetName.Generate(ctx.Child("street"));
        var scifi = Combinators.OneOf<string, object?>("Cruiser", "Frigate", "Interceptor");
        var ship = scifi.Generate(ctx.Child("ship"));
        
        Console.WriteLine($"Modern Street Name as a Sci-Fi Ship Class:");
        Console.WriteLine($"  The {name} {ship}");
    }
}
