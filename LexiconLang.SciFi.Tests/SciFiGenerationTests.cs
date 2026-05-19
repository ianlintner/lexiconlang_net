using LexiconLang.Core;
using LexiconLang.Language;

namespace LexiconLang.SciFi.Tests;

public class SciFiGenerationTests
{
    [Fact]
    public void AllTwelveCulturesAreDefined()
    {
        Assert.NotNull(SciFiCultures.Humanoid);
        Assert.NotNull(SciFiCultures.Insectoid);
        Assert.NotNull(SciFiCultures.Aquatic);
        Assert.NotNull(SciFiCultures.Synth);
        Assert.NotNull(SciFiCultures.BirdPeople);
        Assert.NotNull(SciFiCultures.RockPeople);
        Assert.NotNull(SciFiCultures.Mycoids);
        Assert.NotNull(SciFiCultures.MammalianAlien);
        Assert.NotNull(SciFiCultures.Plantoid);
        Assert.NotNull(SciFiCultures.Reptilian);
        Assert.NotNull(SciFiCultures.Hivemind);
        Assert.NotNull(SciFiCultures.Grayfolk);
    }

    [Fact]
    public void AllCultureIdsAreDistinct()
    {
        var ids = new[]
        {
            SciFiCultures.Humanoid.Id,
            SciFiCultures.Insectoid.Id,
            SciFiCultures.Aquatic.Id,
            SciFiCultures.Synth.Id,
            SciFiCultures.BirdPeople.Id,
            SciFiCultures.RockPeople.Id,
            SciFiCultures.Mycoids.Id,
            SciFiCultures.MammalianAlien.Id,
            SciFiCultures.Plantoid.Id,
            SciFiCultures.Reptilian.Id,
            SciFiCultures.Hivemind.Id,
            SciFiCultures.Grayfolk.Id,
        };

        Assert.Equal(ids.Length, ids.Distinct().Count());
    }

    [Fact]
    public void SciFiMeaningsCoreIsPopulated()
    {
        Assert.NotNull(SciFiMeanings.Core);
        Assert.NotEmpty(SciFiMeanings.Core.Meanings);
    }

    [Fact]
    public void AllCulturesHaveMeaningPacks()
    {
        var all = new[]
        {
            SciFiCultures.Humanoid,
            SciFiCultures.Insectoid,
            SciFiCultures.Aquatic,
            SciFiCultures.Synth,
            SciFiCultures.BirdPeople,
            SciFiCultures.RockPeople,
            SciFiCultures.Mycoids,
            SciFiCultures.MammalianAlien,
            SciFiCultures.Plantoid,
            SciFiCultures.Reptilian,
            SciFiCultures.Hivemind,
            SciFiCultures.Grayfolk,
        };

        foreach (var culture in all)
        {
            Assert.NotEmpty(culture.MeaningPacks);
        }
    }

    [Fact]
    public void SciFiContextIsDeterministic()
    {
        var ctx1 = LexiconContext.Create("scifi-seed");
        var val1 = ctx1.Rng.NextUInt32();

        var ctx2 = LexiconContext.Create("scifi-seed");
        var val2 = ctx2.Rng.NextUInt32();

        Assert.Equal(val1, val2);
    }

    [Theory]
    [InlineData("humanoid")]
    [InlineData("insectoid")]
    [InlineData("aquatic")]
    [InlineData("synth")]
    [InlineData("birdpeople")]
    [InlineData("rockpeople")]
    [InlineData("mycoids")]
    [InlineData("mammalian")]
    [InlineData("plantoid")]
    [InlineData("reptilian")]
    [InlineData("hivemind")]
    [InlineData("grayfolk")]
    public void GivenNameGenerationProducesNonEmptyResult(string cultureKey)
    {
        var culture = cultureKey switch
        {
            "humanoid" => SciFiCultures.Humanoid,
            "insectoid" => SciFiCultures.Insectoid,
            "aquatic" => SciFiCultures.Aquatic,
            "synth" => SciFiCultures.Synth,
            "birdpeople" => SciFiCultures.BirdPeople,
            "rockpeople" => SciFiCultures.RockPeople,
            "mycoids" => SciFiCultures.Mycoids,
            "mammalian" => SciFiCultures.MammalianAlien,
            "plantoid" => SciFiCultures.Plantoid,
            "reptilian" => SciFiCultures.Reptilian,
            "hivemind" => SciFiCultures.Hivemind,
            "grayfolk" => SciFiCultures.Grayfolk,
            _ => throw new ArgumentException(cultureKey)
        };

        var ctx = LexiconContext.Create($"test.{cultureKey}.given");
        var name = Templates.GenerateName(culture, "given", ctx);

        Assert.NotNull(name);
        Assert.False(string.IsNullOrWhiteSpace(name.Form));
        Assert.Equal(culture.Id, name.Language);
    }

    [Theory]
    [InlineData("birdpeople")]
    [InlineData("rockpeople")]
    [InlineData("mycoids")]
    [InlineData("mammalian")]
    [InlineData("plantoid")]
    [InlineData("reptilian")]
    [InlineData("grayfolk")]
    public void SurnameGenerationProducesNonEmptyResult(string cultureKey)
    {
        var culture = cultureKey switch
        {
            "birdpeople" => SciFiCultures.BirdPeople,
            "rockpeople" => SciFiCultures.RockPeople,
            "mycoids" => SciFiCultures.Mycoids,
            "mammalian" => SciFiCultures.MammalianAlien,
            "plantoid" => SciFiCultures.Plantoid,
            "reptilian" => SciFiCultures.Reptilian,
            "grayfolk" => SciFiCultures.Grayfolk,
            _ => throw new ArgumentException(cultureKey)
        };

        var ctx = LexiconContext.Create($"test.{cultureKey}.surname");
        var name = Templates.GenerateName(culture, "surname", ctx);

        Assert.NotNull(name);
        Assert.False(string.IsNullOrWhiteSpace(name.Form));
        Assert.Equal(culture.Id, name.Language);
    }

    [Fact]
    public void NameGenerationIsDeterministic()
    {
        var ctx1 = LexiconContext.Create("det-scifi-seed");
        var name1 = Templates.GenerateName(SciFiCultures.Humanoid, "given", ctx1);

        var ctx2 = LexiconContext.Create("det-scifi-seed");
        var name2 = Templates.GenerateName(SciFiCultures.Humanoid, "given", ctx2);

        Assert.Equal(name1.Form, name2.Form);
        Assert.Equal(name1.Translation, name2.Translation);
    }
}
