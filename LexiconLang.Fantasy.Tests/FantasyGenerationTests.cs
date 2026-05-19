using LexiconLang.Core;

namespace LexiconLang.Fantasy.Tests;

public class FantasyGenerationTests
{
    [Fact]
    public void CanCreateContextForFantasy()
    {
        var ctx = LexiconContext.Create("fantasy-world");
        Assert.NotNull(ctx);
        Assert.NotNull(ctx.Rng);
    }

    [Fact]
    public void FantasySeedIsDeterministic()
    {
        var ctx1 = LexiconContext.Create("same-fantasy");
        var val1 = ctx1.Rng.NextUInt32();

        var ctx2 = LexiconContext.Create("same-fantasy");
        var val2 = ctx2.Rng.NextUInt32();

        Assert.Equal(val1, val2);
    }

    [Fact]
    public void FantasyContextCanHaveChildren()
    {
        var root = LexiconContext.Create("world");
        var npc = root.Child("npc-1");
        var spell = root.Child("spell-1");

        Assert.NotEqual(npc.Rng.NextUInt32(), spell.Rng.NextUInt32());
    }

    [Fact]
    public void FantasyContextTagsWork()
    {
        var ctx = LexiconContext.Create("fantasy").WithTags("high-magic", "medieval");
        Assert.Contains("high-magic", ctx.Tags);
        Assert.Contains("medieval", ctx.Tags);
    }

    [Fact]
    public void FantasyContextChildInheritsParentSettings()
    {
        var root = LexiconContext.Create("world").WithTags("fantasy");
        var child = root.Child("realm");
        
        var parentVal = root.Rng.NextUInt32();
        var childVal = child.Rng.NextUInt32();
        
        Assert.NotEqual(parentVal, childVal);
    }
}
