using LexiconLang.Core;

namespace LexiconLang.Core.Tests;

public class CombinatorTests
{
    private sealed record Npc(string Name, int Age);

    [Fact]
    public void OneOfIsDeterministic()
    {
        var generator = Combinators.OneOf<string, object?>("a", "b", "c", "d");
        var ctx1 = LexiconContext.Create("k");
        var ctx2 = LexiconContext.Create("k");
        Assert.Equal(generator.Generate(ctx1), generator.Generate(ctx2));
    }

    [Fact]
    public void WeightedListHonorsWeights()
    {
        var generator = Combinators.WeightedList<string, object?>(
            new Dictionary<string, double> { ["a"] = 1, ["b"] = 9 });
        var ctx = LexiconContext.Create("stats");
        var aCount = 0;
        const int n = 5000;
        for (var i = 0; i < n; i++)
        {
            if (generator.Generate(ctx.Child($"i:{i}")) == "a") aCount++;
        }

        var ratio = (double)aCount / n;
        Assert.InRange(ratio, 0.06, 0.16);
    }

    [Fact]
    public void ComposeSameSeedSameOutput()
    {
        var nameGen = Combinators.OneOf<string, object?>("Alice", "Bob", "Carol");
        var ageGen = Combinators.IntRange<object?>(20, 60);
        var npc = Combinators.Compose<Npc, object?>(
            "test.npc",
            ctx => new Npc(
                nameGen.Generate(ctx.Child("name")),
                ageGen.Generate(ctx.Child("age"))));

        var a = npc.Generate(LexiconContext.Create("world-1"));
        var b = npc.Generate(LexiconContext.Create("world-1"));
        Assert.Equal(a, b);
    }

    [Fact]
    public void ComposeFieldReorderDoesNotChangePerFieldOutput()
    {
        var orderA = Combinators.Compose<Npc, object?>(
            "t1",
            ctx => new Npc(
                Combinators.OneOf<string, object?>("X", "Y", "Z").Generate(ctx.Child("name")),
                Combinators.IntRange<object?>(1, 100).Generate(ctx.Child("age"))));

        var orderB = Combinators.Compose<Npc, object?>(
            "t1",
            ctx => new Npc(
                Combinators.OneOf<string, object?>("X", "Y", "Z").Generate(ctx.Child("name")),
                Combinators.IntRange<object?>(1, 100).Generate(ctx.Child("age"))));

        var ctx = LexiconContext.Create("reorder");
        var a = orderA.Generate(ctx);
        var b = orderB.Generate(ctx);
        Assert.Equal(a.Name, b.Name);
        Assert.Equal(a.Age, b.Age);
    }

    [Fact]
    public void RepeatFixedCountSameSeedSameArray()
    {
        var generator = Combinators.Repeat(
            Combinators.OneOf<string, object?>("a", "b", "c"),
            new CountSpec(Count: 5));
        var a = generator.Generate(LexiconContext.Create("rep"));
        var b = generator.Generate(LexiconContext.Create("rep"));
        Assert.Equal(a, b);
        Assert.Equal(5, a.Count);
    }

    [Fact]
    public void RepeatRangeSameSeedSameArray()
    {
        var generator = Combinators.Repeat(
            Combinators.OneOf<string, object?>("a", "b"),
            new CountSpec(Min: 3, Max: 7));
        var a = generator.Generate(LexiconContext.Create("rep-range"));
        var b = generator.Generate(LexiconContext.Create("rep-range"));
        Assert.Equal(a, b);
        Assert.InRange(a.Count, 3, 7);
    }

    [Fact]
    public void HierarchicalContextProducesSameOutputForSamePath()
    {
        var generator = Combinators.OneOf<string, object?>("a", "b", "c", "d", "e");
        var root1 = LexiconContext.Create("world");
        var root2 = LexiconContext.Create("world");

        var path1 = generator.Generate(root1.Child("region:1").Child("settlement:5").Child("npc:7"));
        root2.Child("region:99").Child("settlement:0");
        var path2 = generator.Generate(root2.Child("region:1").Child("settlement:5").Child("npc:7"));

        Assert.Equal(path1, path2);
    }
}
