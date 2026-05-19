using System;
using LexiconLang.Core;
using LexiconLang.Grammar;

namespace LexiconLang.Examples;

public static class Example05_CustomGrammar
{
    public static void Run()
    {
        var rules = new GrammarRules<object?>
        {
            { "start", new Weighted<string>[]
                {
                    new ("#prefix.capitalize# #element.capitalize# #form.capitalize#", 5),
                    new ("#name.capitalize#'s #element.capitalize# #form.capitalize#", 3),
                    new ("the #adj.capitalize# #form.capitalize#", 2)
                }
            },
            { "prefix", new string[] { "lesser", "greater", "true", "binding", "shattering", "whispering" } },
            { "element", new string[] { "fire", "frost", "shadow", "sun", "echo", "thorn", "iron" } },
            { "form", new string[] { "bolt", "ward", "veil", "lash", "stride", "gaze", "chord" } },
            { "adj", new string[] { "unsleeping", "patient", "errant", "violet", "broken" } },
            { "name", new string[] { "aelior", "varian", "morwen", "thessaly" } }
        };
        var spellName = new Grammar<object?>(rules);

        var ctx = LexiconContext.Create("spellbook-1");
        Console.WriteLine("Spells:");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"  • {spellName.Generate(ctx.Child($"s:{i}"))}");
        }
    }
}
