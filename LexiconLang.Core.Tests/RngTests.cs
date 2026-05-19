using LexiconLang.Core;

namespace LexiconLang.Core.Tests;

public class RngTests
{
    [Fact]
    public void IsDeterministicFromStringSeed()
    {
        var a = Rng.Create("hello");
        var b = Rng.Create("hello");
        for (var i = 0; i < 100; i++)
        {
            Assert.Equal(a.NextUInt32(), b.NextUInt32());
        }
    }

    [Fact]
    public void DiffersAcrossDistinctSeeds()
    {
        var a = Rng.Create("hello").NextUInt32();
        var b = Rng.Create("world").NextUInt32();
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void ForkIsDeterministic()
    {
        var a = Rng.Create("seed").Fork("region:1");
        var b = Rng.Create("seed").Fork("region:1");
        for (var i = 0; i < 50; i++)
        {
            Assert.Equal(a.NextUInt32(), b.NextUInt32());
        }
    }

    [Fact]
    public void ForkDoesNotConsumeParentStream()
    {
        var parent1 = Rng.Create("seed");
        var childA1 = parent1.Fork("a");
        var childB1 = parent1.Fork("b");

        var parent2 = Rng.Create("seed");
        var childB2 = parent2.Fork("b");
        parent2.NextUInt32();
        parent2.NextUInt32();
        var childA2 = parent2.Fork("a");

        Assert.Equal(childA1.NextUInt32(), childA2.NextUInt32());
        Assert.Equal(childB1.NextUInt32(), childB2.NextUInt32());
    }

    [Fact]
    public void StateRoundTrips()
    {
        var a = Rng.Create("roundtrip");
        for (var i = 0; i < 17; i++) a.NextUInt32();
        var snap = a.State();
        var b = new Sfc32(snap);
        for (var i = 0; i < 50; i++)
        {
            Assert.Equal(a.NextUInt32(), b.NextUInt32());
        }
    }

    [Fact]
    public void NextStaysInRange()
    {
        var r = Rng.Create("range");
        for (var i = 0; i < 1000; i++)
        {
            var x = r.Next();
            Assert.InRange(x, 0, 0.9999999999999999d);
        }
    }

    [Fact]
    public void NextIntProducesValuesInRange()
    {
        var r = Rng.Create("intRange");
        for (var i = 0; i < 1000; i++)
        {
            var x = r.NextInt(5, 10);
            Assert.InRange(x, 5, 9);
        }
    }
}
