import os

files = {
    "Program.cs": """using System;

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
        Console.WriteLine($"\\n--- {name} ---");
        action();
    }
}
""",
    "Example01_Quickstart.cs": """using System;
using LexiconLang.Core;
using LexiconLang.Fantasy;

namespace LexiconLang.Examples;

public static class Example01_Quickstart
{
    public static void Run()
    {
        // 01-quickstart — the smallest possible useful program.
        var ctx = Context<object?>.Root("hello-world");

        Console.WriteLine("A random Encounter:");
        var faction = FantasyEncounters.FactionGenerator.Generate(ctx.Child("faction"));
        var title = FantasyEncounters.TitleGenerator.Generate(ctx.Child("title"));
        Console.WriteLine($"  The {title.Form} of {faction.Form}");
    }
}
""",
    "Example02_BatchPatrons.cs": """using System;
using System.Linq;
using LexiconLang.Core;
using LexiconLang.Fantasy;

namespace LexiconLang.Examples;

public static class Example02_BatchPatrons
{
    public static void Run()
    {
        // 02-batch-patrons — generating a list of items at once.
        var tavernSeed = Context<object?>.Root("the-gilded-anchor");

        // Fixed-size party.
        var partyGen = Combinators.Repeat(FantasyEncounters.TitleGenerator, 6);
        var partyOfSix = partyGen.Generate(tavernSeed.Child("party"));

        Console.WriteLine("Adventuring party titles:");
        foreach (var member in partyOfSix)
        {
            Console.WriteLine($"  • {member.Form}");
        }

        // Variable-size patrons (between 5 and 12 — count is itself RNG-driven).
        var patronsGen = Combinators.Repeat(FantasyEncounters.TitleGenerator, new CountSpec(5, 12));
        var patrons = patronsGen.Generate(tavernSeed.Child("patrons"));

        Console.WriteLine($"\\n{patrons.Count()} patrons in the tavern tonight:");
        foreach (var p in patrons)
        {
            Console.WriteLine($"  • {p.Form}");
        }
    }
}
""",
    "Example03_WorldTree.cs": """using System;
using LexiconLang.Core;
using LexiconLang.Fantasy;
using LexiconLang.Language;
using LexiconLang.Grammar;

namespace LexiconLang.Examples;

public static class Example03_WorldTree
{
    public static void Run()
    {
        var world = Context<object?>.Root("campaign-of-iron");

        string[] regionNames = { "Northmarch", "Sunken Coast", "Ember Reach" };
        var ruler = FantasyEncounters.TitleGenerator.Generate(world.Child("ruling-faction"));

        Console.WriteLine($"World: ruled by {ruler.Form}");
        Console.WriteLine(new string('-', 60));

        for (int r = 0; r < regionNames.Length; r++)
        {
            var region = world.Child($"region:{r}");
            var capital = Templates.GenerateName(FantasyCultures.Dwarvish, "given", region.Child("capital"));
            
            Console.WriteLine($"\\n{regionNames[r]} — capital: {capital.Form}");

            var settlements = Combinators.Repeat(FantasyEncounters.TitleGenerator, 2).Generate(region.Child("settlements"));
            foreach (var s in settlements)
            {
                Console.WriteLine($"  {s.Form}");
            }
        }
    }
}
""",
    "Example04_CustomGenerator.cs": """using System;
using LexiconLang.Core;
using LexiconLang.Fantasy;

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
                ctx.Rng.Next(1, 40)
            );
        });

        var rootCtx = Context<object?>.Root("iron-watch");
        Console.WriteLine("The Iron Watch, current roster:\\n");
        for (int i = 0; i < 5; i++)
        {
            var k = ironKnight.Generate(rootCtx.Child($"watch:{i}"));
            Console.WriteLine($"  {k.Rank.PadRight(15)} {k.Name.PadRight(20)} of House {k.House} — \\"{k.Sword}\\"");
        }
    }
}
""",
    "Example05_CustomGrammar.cs": """using System;
using LexiconLang.Core;
using LexiconLang.Grammar;

namespace LexiconLang.Examples;

public static class Example05_CustomGrammar
{
    public static void Run()
    {
        var spellName = new LexiconLang.Grammar.Grammar<object?>();
        spellName.AddRule("start", new Weighted<RuleValue<object?>>[]
        {
            new ("#prefix.capitalize# #element.capitalize# #form.capitalize#", 5),
            new ("#name.capitalize#'s #element.capitalize# #form.capitalize#", 3),
            new ("the #adj.capitalize# #form.capitalize#", 2)
        });
        spellName.AddRule("prefix", new string[] { "lesser", "greater", "true", "binding", "shattering", "whispering" });
        spellName.AddRule("element", new string[] { "fire", "frost", "shadow", "sun", "echo", "thorn", "iron" });
        spellName.AddRule("form", new string[] { "bolt", "ward", "veil", "lash", "stride", "gaze", "chord" });
        spellName.AddRule("adj", new string[] { "unsleeping", "patient", "errant", "violet", "broken" });
        spellName.AddRule("name", new string[] { "aelior", "varian", "morwen", "thessaly" });

        var ctx = Context<object?>.Root("spellbook-1");
        Console.WriteLine("Spells:");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"  • {spellName.Generate(ctx.Child($"s:{i}"))}");
        }
    }
}
""",
    "Example06_CustomMarkov.cs": """using System;
using LexiconLang.Core;
using LexiconLang.Markov;
using System.Linq;

namespace LexiconLang.Examples;

public static class Example06_CustomMarkov
{
    public static void Run()
    {
        var corpus = new[]
        {
            "aberystwyth", "caernarfon", "carmarthen", "ceredigion", "conwy",
            "gwynedd", "llandudno", "llangefni", "llanrwst", "machynlleth"
        };
        var entries = corpus.Select(x => new TrainEntry(x));

        var model = Trainer.Train(entries, new TrainOptions(Order: 3, MinLength: 5, MaxLength: 12, RejectSubstringsOfLength: 6));
        Console.WriteLine("Model trained.");

        var townName = new MarkovGenerator<object?>(model, "welsh.town");
        var ctx = Context<object?>.Root("cymru");

        Console.WriteLine("10 generated town names:");
        var names = Combinators.Repeat(townName, 10).Generate(ctx);
        foreach (var n in names) Console.WriteLine($"  • {n}");
    }
}
""",
    "Example07_SeedAndReroll.cs": """using System;
using System.Collections.Generic;
using LexiconLang.Core;
using LexiconLang.Fantasy;

namespace LexiconLang.Examples;

public static class Example07_SeedAndReroll
{
    public static void Run()
    {
        Console.WriteLine("Simulating saving and rerolling exact subsets.\\n");
        var saveRerolls = new Dictionary<string, int>();

        string GetSettlement(string path)
        {
            saveRerolls.TryGetValue(path, out var v);
            var ctx = Context<object?>.Root("shared-realm").Child(path).Child($"v:{v}");
            return FantasyEncounters.TitleGenerator.Generate(ctx).Form;
        }

        Console.WriteLine("Three titles initially:");
        for (int i = 0; i < 3; i++)
            Console.WriteLine($"  Title {i}  →  {GetSettlement($"title:{i}")}");

        Console.WriteLine("\\nPlayer rerolls Title 1:");
        saveRerolls["title:1"] = 1;

        for (int i = 0; i < 3; i++)
        {
            var note = i == 1 ? "  ← REROLLED" : "";
            Console.WriteLine($"  Title {i}  →  {GetSettlement($"title:{i}")}{note}");
        }
    }
}
""",
    "Example08_CrossGenre.cs": """using System;
using LexiconLang.Core;
using LexiconLang.Modern;

namespace LexiconLang.Examples;

public static class Example08_CrossGenre
{
    public static void Run()
    {
        var ctx = Context<object?>.Root("the-ironwake");
        Console.WriteLine("Cross-Genre Generation:\\n");
        
        var name = LexiconLang.Modern.ModernNames.StreetName.Generate(ctx.Child("street"));
        var scifi = Combinators.OneOf<string, object?>("Cruiser", "Frigate", "Interceptor");
        var ship = scifi.Generate(ctx.Child("ship"));
        
        Console.WriteLine($"Modern Street Name as a Sci-Fi Ship Class:");
        Console.WriteLine($"  The {name.Form} {ship}");
    }
}
""",
    "Example09_Glyphs.cs": """using System;
using LexiconLang.Core;
using LexiconLang.Language;
using LexiconLang.Fantasy;
using LexiconLang.Glyphs;

namespace LexiconLang.Examples;

public static class Example09_Glyphs
{
    public static void Run()
    {
        var ctx = Context<object?>.Root("glyph-demo");

        var dwarfName = Templates.GenerateName(FantasyCultures.Dwarvish, "given", ctx.Child("dwarf"));
        var visualSystems = FantasyCultures.Dwarvish.VisualGlyphSystems;
        var phoneticGlyphs = (visualSystems as LexiconLang.Glyphs.VisualGlyphSystems)?.Phonetic;
        
        if (phoneticGlyphs == null) return;
        var dwarfGlyphs = GlyphGenerator.GlyphsFor(dwarfName, phoneticGlyphs, ctx.Child("dwarf"));

        Console.WriteLine($"Dwarvish — {dwarfName.Form} ({dwarfName.Translation})");
        Console.WriteLine($"  {dwarfGlyphs.Phonetic?.Length} runes, one per phoneme pair");
        if (dwarfGlyphs.Phonetic != null)
        {
            for (var i = 0; i < dwarfGlyphs.Phonetic.Length; i++)
            {
                var svg = dwarfGlyphs.Phonetic[i].Svg ?? "";
                svg = svg.Substring(0, Math.Min(70, svg.Length));
                Console.WriteLine($"  [{i}] {svg}…");
            }
        }
    }
}
"""
}

target_dir = "/Users/ianlintner/Projects/lexiconlang_net/examples/LexiconLang.Examples"

for filename, content in files.items():
    path = os.path.join(target_dir, filename)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)

print("Files written successfully in", target_dir)
