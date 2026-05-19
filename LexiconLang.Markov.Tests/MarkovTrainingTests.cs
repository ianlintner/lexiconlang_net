using LexiconLang.Core;
using LexiconLang.Markov;

namespace LexiconLang.Markov.Tests;

public class MarkovTrainingTests
{
    [Fact]
    public void TrainerBuildsFromCorpus()
    {
        var corpus = new[]
        {
            new TrainEntry("cat"),
            new TrainEntry("dog"),
            new TrainEntry("bat"),
            new TrainEntry("hat")
        };

        var model = Trainer.Train(corpus, new TrainOptions(Order: 2));
        Assert.NotNull(model);
    }

    [Fact]
    public void SamplerGeneratesFromModel()
    {
        var corpus = new[]
        {
            new TrainEntry("hello"),
            new TrainEntry("world"),
            new TrainEntry("help"),
            new TrainEntry("hold")
        };

        var model = Trainer.Train(corpus);
        var rng = Rng.Create("markov");
        var sample = Sampler.Sample(model, rng);

        Assert.NotNull(sample);
        Assert.NotEmpty(sample);
    }

    [Fact]
    public void SamplingIsDeterministic()
    {
        var corpus = new[]
        {
            new TrainEntry("apple"),
            new TrainEntry("apply"),
            new TrainEntry("ample"),
            new TrainEntry("example")
        };

        var model = Trainer.Train(corpus);
        var rng1 = Rng.Create("sample1");
        var sample1 = Sampler.Sample(model, rng1);

        var rng2 = Rng.Create("sample1");
        var sample2 = Sampler.Sample(model, rng2);

        Assert.Equal(sample1, sample2);
    }

    [Fact]
    public void DifferentSeedsProduceDifferentSamples()
    {
        var corpus = new[]
        {
            new TrainEntry("cat"),
            new TrainEntry("dog"),
            new TrainEntry("rat"),
            new TrainEntry("bat"),
            new TrainEntry("hat"),
            new TrainEntry("mat")
        };

        var model = Trainer.Train(corpus);
        var rng1 = Rng.Create("seed1");
        var sample1 = Sampler.Sample(model, rng1);

        var rng2 = Rng.Create("seed2");
        var sample2 = Sampler.Sample(model, rng2);

        // Different seeds should produce different samples most of the time
        Assert.NotEqual(sample1, sample2);
    }

    [Fact]
    public void TrainerHandlesWeightedEntries()
    {
        var corpus = new[]
        {
            new TrainEntry("common", 10),
            new TrainEntry("rare", 1),
            new TrainEntry("very-rare", 0.1)
        };

        var model = Trainer.Train(corpus);
        Assert.NotNull(model);
    }

    [Fact]
    public void TrainerHandlesCustomOptions()
    {
        var corpus = new[]
        {
            new TrainEntry("fire"),
            new TrainEntry("water"),
            new TrainEntry("earth"),
            new TrainEntry("air")
        };

        var options = new TrainOptions(
            Order: 2,
            MinLength: 2,
            MaxLength: 10,
            Lowercase: true);

        var model = Trainer.Train(corpus, options);
        Assert.NotNull(model);
    }

    [Fact]
    public void TrainerRaisesOnEmptyCorpus()
    {
        var corpus = Array.Empty<TrainEntry>();
        Assert.Throws<InvalidOperationException>(() => Trainer.Train(corpus));
    }
}
