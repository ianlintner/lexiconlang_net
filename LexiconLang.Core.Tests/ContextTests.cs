using LexiconLang.Core;

namespace LexiconLang.Core.Tests;

public class ContextTests
{
    [Fact]
    public void ContextHasRng()
    {
        var ctx = LexiconContext.Create("test");
        Assert.NotNull(ctx.Rng);
    }

    [Fact]
    public void ContextCanCreateChild()
    {
        var ctx = LexiconContext.Create("root");
        var child = ctx.Child("child");

        Assert.NotNull(child);
        Assert.NotEqual(ctx.Rng, child.Rng);
    }

    [Fact]
    public void ContextScopeTracksPath()
    {
        var ctx = LexiconContext.Create("root");
        var child = ctx.Child("child");
        var grandchild = child.Child("grandchild");

        // Root context has no scope labels; the seed is an ID, not a path segment
        Assert.Empty(ctx.Scope);
        Assert.Contains("child", child.Scope);
        Assert.True(grandchild.Scope.Count >= 2);
    }

    [Fact]
    public void ContextCanHaveTags()
    {
        var ctx = LexiconContext.Create("test");
        var withTags = ctx.WithTags("tag1", "tag2");

        Assert.Contains("tag1", withTags.Tags);
        Assert.Contains("tag2", withTags.Tags);
    }

    [Fact]
    public void DifferentSeedsCreateDifferentRng()
    {
        var ctx1 = LexiconContext.Create("seed1");
        var ctx2 = LexiconContext.Create("seed2");

        var v1 = ctx1.Rng.NextUInt32();
        var v2 = ctx2.Rng.NextUInt32();

        Assert.NotEqual(v1, v2);
    }

    [Fact]
    public void SameSeedCreatesDeterministicRng()
    {
        var ctx1 = LexiconContext.Create("same");
        var ctx2 = LexiconContext.Create("same");

        var v1 = ctx1.Rng.NextUInt32();
        var v2 = ctx2.Rng.NextUInt32();

        Assert.Equal(v1, v2);
    }

    [Fact]
    public void ChildrenHaveDifferentRng()
    {
        var root = LexiconContext.Create("root");
        var childA = root.Child("a");
        var childB = root.Child("b");

        var valA = childA.Rng.NextUInt32();
        var valB = childB.Rng.NextUInt32();

        Assert.NotEqual(valA, valB);
    }

    [Fact]
    public void SameChildLabelProduceSameRng()
    {
        var root1 = LexiconContext.Create("root");
        var root2 = LexiconContext.Create("root");

        var child1 = root1.Child("label");
        var child2 = root2.Child("label");

        var val1 = child1.Rng.NextUInt32();
        var val2 = child2.Rng.NextUInt32();

        Assert.Equal(val1, val2);
    }
}
