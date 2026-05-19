using LexiconLang.Core;

namespace LexiconLang.SciFi.Tests;

public class SciFiGenerationTests
{
    [Fact]
    public void SciFiCulturesAreDefined()
    {
        Assert.NotNull(SciFiCultures.Humanoid);
        Assert.NotNull(SciFiCultures.Insectoid);
        Assert.NotNull(SciFiCultures.Aquatic);
        Assert.NotNull(SciFiCultures.Synth);
    }

    [Fact]
    public void SciFiCultureIdsAreDistinct()
    {
        var ids = new[]
        {
            SciFiCultures.Humanoid.Id,
            SciFiCultures.Insectoid.Id,
            SciFiCultures.Aquatic.Id,
            SciFiCultures.Synth.Id,
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
    public void SciFiCultureHasMeaningPacks()
    {
        Assert.NotEmpty(SciFiCultures.Humanoid.MeaningPacks);
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
}
