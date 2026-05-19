using LexiconLang.Core;
using LexiconLang.Language;

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

    [Fact]
    public void AllTenCulturesAreDefined()
    {
        Assert.NotNull(FantasyCultures.Dwarvish);
        Assert.NotNull(FantasyCultures.Elvish);
        Assert.NotNull(FantasyCultures.Orcish);
        Assert.NotNull(FantasyCultures.Halfling);
        Assert.NotNull(FantasyCultures.Draconic);
        Assert.NotNull(FantasyCultures.Plantoid);
        Assert.NotNull(FantasyCultures.Mycanoid);
        Assert.NotNull(FantasyCultures.Celestial);
        Assert.NotNull(FantasyCultures.Fey);
        Assert.NotNull(FantasyCultures.Tiefling);
    }

    [Fact]
    public void AllCultureIdsAreDistinct()
    {
        var ids = new[]
        {
            FantasyCultures.Dwarvish.Id,
            FantasyCultures.Elvish.Id,
            FantasyCultures.Orcish.Id,
            FantasyCultures.Halfling.Id,
            FantasyCultures.Draconic.Id,
            FantasyCultures.Plantoid.Id,
            FantasyCultures.Mycanoid.Id,
            FantasyCultures.Celestial.Id,
            FantasyCultures.Fey.Id,
            FantasyCultures.Tiefling.Id,
        };

        Assert.Equal(ids.Length, ids.Distinct().Count());
    }

    [Theory]
    [InlineData("fantasy.dwarvish")]
    [InlineData("fantasy.elvish")]
    [InlineData("fantasy.orcish")]
    [InlineData("fantasy.halfling")]
    [InlineData("fantasy.draconic")]
    [InlineData("fantasy.plantoid")]
    [InlineData("fantasy.mycanoid")]
    [InlineData("fantasy.celestial")]
    [InlineData("fantasy.fey")]
    [InlineData("fantasy.tiefling")]
    public void AllCulturesHaveExpectedIdPrefix(string expectedId)
    {
        var all = new[]
        {
            FantasyCultures.Dwarvish,
            FantasyCultures.Elvish,
            FantasyCultures.Orcish,
            FantasyCultures.Halfling,
            FantasyCultures.Draconic,
            FantasyCultures.Plantoid,
            FantasyCultures.Mycanoid,
            FantasyCultures.Celestial,
            FantasyCultures.Fey,
            FantasyCultures.Tiefling,
        };

        Assert.Contains(all, c => c.Id == expectedId);
    }

    [Theory]
    [InlineData("dwarvish", "given")]
    [InlineData("dwarvish", "surname")]
    [InlineData("dwarvish", "settlement")]
    [InlineData("elvish", "given")]
    [InlineData("elvish", "surname")]
    [InlineData("orcish", "given")]
    [InlineData("orcish", "surname")]
    [InlineData("halfling", "given")]
    [InlineData("halfling", "surname")]
    [InlineData("draconic", "given")]
    [InlineData("plantoid", "given")]
    [InlineData("plantoid", "surname")]
    [InlineData("plantoid", "settlement")]
    [InlineData("mycanoid", "given")]
    [InlineData("mycanoid", "surname")]
    [InlineData("mycanoid", "settlement")]
    [InlineData("celestial", "given")]
    [InlineData("celestial", "surname")]
    [InlineData("celestial", "settlement")]
    [InlineData("fey", "given")]
    [InlineData("fey", "surname")]
    [InlineData("tiefling", "given")]
    [InlineData("tiefling", "surname")]
    public void NameGenerationProducesNonEmptyResult(string cultureKey, string nameKind)
    {
        var culture = cultureKey switch
        {
            "dwarvish" => FantasyCultures.Dwarvish,
            "elvish" => FantasyCultures.Elvish,
            "orcish" => FantasyCultures.Orcish,
            "halfling" => FantasyCultures.Halfling,
            "draconic" => FantasyCultures.Draconic,
            "plantoid" => FantasyCultures.Plantoid,
            "mycanoid" => FantasyCultures.Mycanoid,
            "celestial" => FantasyCultures.Celestial,
            "fey" => FantasyCultures.Fey,
            "tiefling" => FantasyCultures.Tiefling,
            _ => throw new ArgumentException(cultureKey)
        };

        var ctx = LexiconContext.Create($"test.{cultureKey}.{nameKind}");
        var name = Templates.GenerateName(culture, nameKind, ctx);

        Assert.NotNull(name);
        Assert.False(string.IsNullOrWhiteSpace(name.Form));
        Assert.Equal(culture.Id, name.Language);
    }

    [Fact]
    public void NameGenerationIsDeterministic()
    {
        var ctx1 = LexiconContext.Create("det-seed");
        var name1 = Templates.GenerateName(FantasyCultures.Elvish, "given", ctx1);

        var ctx2 = LexiconContext.Create("det-seed");
        var name2 = Templates.GenerateName(FantasyCultures.Elvish, "given", ctx2);

        Assert.Equal(name1.Form, name2.Form);
        Assert.Equal(name1.Translation, name2.Translation);
    }

    [Fact]
    public void DifferentSeedsDifferentNames()
    {
        var ctx1 = LexiconContext.Create("seed-a");
        var name1 = Templates.GenerateName(FantasyCultures.Elvish, "given", ctx1);

        var ctx2 = LexiconContext.Create("seed-b");
        var name2 = Templates.GenerateName(FantasyCultures.Elvish, "given", ctx2);

        // Different seeds should virtually always yield different names
        Assert.False(name1.Form == name2.Form && name1.Translation == name2.Translation);
    }

    [Fact]
    public void AllCulturesHaveMeaningPacks()
    {
        var all = new[]
        {
            FantasyCultures.Dwarvish,
            FantasyCultures.Elvish,
            FantasyCultures.Orcish,
            FantasyCultures.Halfling,
            FantasyCultures.Draconic,
            FantasyCultures.Plantoid,
            FantasyCultures.Mycanoid,
            FantasyCultures.Celestial,
            FantasyCultures.Fey,
            FantasyCultures.Tiefling,
        };

        foreach (var culture in all)
        {
            Assert.NotEmpty(culture.MeaningPacks);
        }
    }

    [Fact]
    public void GenerateEncounterProducesPopulatedObject()
    {
        var ctx = LexiconContext.Create("encounter-seed");
        var enc = FantasyEncounters.MakeEncounter(ctx);

        Assert.False(string.IsNullOrEmpty(enc.Faction));
        Assert.False(string.IsNullOrEmpty(enc.Leader));
        Assert.False(string.IsNullOrEmpty(enc.Title));
        Assert.False(string.IsNullOrEmpty(enc.Motive));
        Assert.False(string.IsNullOrEmpty(enc.Description));
        Assert.False(string.IsNullOrEmpty(enc.Summary));
    }

    [Fact]
    public void GenerateEncounterIsDeterministic()
    {
        var ctx1 = LexiconContext.Create("det-enc-seed");
        var enc1 = FantasyEncounters.MakeEncounter(ctx1);

        var ctx2 = LexiconContext.Create("det-enc-seed");
        var enc2 = FantasyEncounters.MakeEncounter(ctx2);

        Assert.Equal(enc1.Summary, enc2.Summary);
    }
}
