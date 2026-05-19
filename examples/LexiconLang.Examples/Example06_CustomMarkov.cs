using System;
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

        var townName = Sampler.AsGenerator<object?>(model);
        var ctx = LexiconContext.Create("cymru");

        Console.WriteLine("10 generated town names:");
        var names = Combinators.Repeat(townName, new CountSpec(Count: 10)).Generate(ctx);
        foreach (var n in names) Console.WriteLine($"  • {n}");
    }
}
