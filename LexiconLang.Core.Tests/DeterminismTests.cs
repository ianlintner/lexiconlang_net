using LexiconLang.Core;

namespace LexiconLang.Core.Tests;

/// <summary>
/// Tests to verify deterministic RNG behavior with seeded contexts.
/// </summary>
public class DeterminismTests
{
    [Fact]
    public void IdenticalSeedsProduceSameSequence()
    {
        const int iterations = 100;
        var seed = "determinism-test";

        var seq1 = new List<uint>();
        var rng1 = Rng.Create(seed);
        for (var i = 0; i < iterations; i++)
        {
            seq1.Add(rng1.NextUInt32());
        }

        var seq2 = new List<uint>();
        var rng2 = Rng.Create(seed);
        for (var i = 0; i < iterations; i++)
        {
            seq2.Add(rng2.NextUInt32());
        }

        Assert.Equal(seq1, seq2);
    }

    [Fact]
    public void DifferentSeedsProduceDifferentSequences()
    {
        var rng1 = Rng.Create("seed1");
        var rng2 = Rng.Create("seed2");

        var v1 = rng1.NextUInt32();
        var v2 = rng2.NextUInt32();

        Assert.NotEqual(v1, v2);
    }

    [Fact]
    public void RngForkProducesDeterministicChild()
    {
        var rng1 = Rng.Create("parent");
        var fork1 = rng1.Fork("child");
        var val1 = fork1.NextUInt32();

        var rng2 = Rng.Create("parent");
        var fork2 = rng2.Fork("child");
        var val2 = fork2.NextUInt32();

        Assert.Equal(val1, val2);
    }

    [Fact]
    public void DifferentForkLabelsCauseVariation()
    {
        var rng = Rng.Create("parent");
        var fork1 = rng.Fork("fork1");
        var fork2 = rng.Fork("fork2");

        var val1 = fork1.NextUInt32();
        var val2 = fork2.NextUInt32();

        Assert.NotEqual(val1, val2);
    }

    [Fact]
    public void RngForkDoesNotConsumeParentStream()
    {
        var rng = Rng.Create("parent");
        // Advance a few steps so state is non-trivial
        _ = rng.NextUInt32();

        // Capture state before fork
        var stateBefore = rng.State();

        var child = rng.Fork("child");
        _ = child.NextUInt32();

        // Fork must not advance the parent stream
        var stateAfter = rng.State();
        Assert.Equal(stateBefore, stateAfter);
    }

    [Fact]
    public void HashingIsDeterministic()
    {
        var hash1 = Hashing.Fnv1A64("test");
        var hash2 = Hashing.Fnv1A64("test");

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void DifferentStringsProduceDifferentHashes()
    {
        var hash1 = Hashing.Fnv1A64("test1");
        var hash2 = Hashing.Fnv1A64("test2");

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void ContextChildrenAreDeterministic()
    {
        var root1 = LexiconContext.Create("root");
        var child1 = root1.Child("child");
        var val1 = child1.Rng.NextUInt32();

        var root2 = LexiconContext.Create("root");
        var child2 = root2.Child("child");
        var val2 = child2.Rng.NextUInt32();

        Assert.Equal(val1, val2);
    }

    [Fact]
    public void SampleIsReproducible()
    {
        var items = new[] { "a", "b", "c", "d", "e" };
        var weighted = Sample.NormalizeWeights<string>(items);
        var table1 = Sample.BuildAliasTable(weighted);

        var rng1 = Rng.Create("sample");
        var item1 = Sample.SampleAlias(table1, rng1);

        var table2 = Sample.BuildAliasTable(weighted);
        var rng2 = Rng.Create("sample");
        var item2 = Sample.SampleAlias(table2, rng2);

        Assert.Equal(item1, item2);
    }

    [Fact]
    public void SamplePicksValidItem()
    {
        var items = new[] { "apple", "banana", "cherry" };
        var weighted = Sample.NormalizeWeights<string>(items);
        var table = Sample.BuildAliasTable(weighted);

        var rng = Rng.Create("pick");
        var item = Sample.SampleAlias(table, rng);

        Assert.Contains(item, items);
    }
}
