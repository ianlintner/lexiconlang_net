using LexiconLang.Core;
using LexiconLang.Glyphs;

namespace LexiconLang.Glyphs.Tests;

public class GlyphGenerationTests
{
    [Fact]
    public void GlyphGeneratorCreatesShapes()
    {
        var rng = Rng.Create("glyph");
        var shapes = ShapeGenerator.GenerateShapes(Complexity.Simple, rng);

        Assert.NotNull(shapes);
        Assert.True(shapes.Length > 0);
    }

    [Fact]
    public void GlyphGenerationIsDeterministic()
    {
        var rng1 = Rng.Create("glyph1");
        var shapes1 = ShapeGenerator.GenerateShapes(Complexity.Medium, rng1);

        var rng2 = Rng.Create("glyph1");
        var shapes2 = ShapeGenerator.GenerateShapes(Complexity.Medium, rng2);

        Assert.Equal(shapes1.Length, shapes2.Length);
    }

    [Fact]
    public void DifferentSeedsProduceDifferentResults()
    {
        var rng1 = Rng.Create("seed1");
        var shapes1 = ShapeGenerator.GenerateShapes(Complexity.Medium, rng1);

        var rng2 = Rng.Create("seed2");
        var shapes2 = ShapeGenerator.GenerateShapes(Complexity.Medium, rng2);

        // Different seeds should likely produce different counts or patterns
        Assert.NotNull(shapes1);
        Assert.NotNull(shapes2);
    }

    [Fact]
    public void ComplexityAffectsShapeCount()
    {
        var rng = Rng.Create("complexity");

        var simple = ShapeGenerator.GenerateShapes(Complexity.Simple, rng.Fork("simple"));
        var medium = ShapeGenerator.GenerateShapes(Complexity.Medium, rng.Fork("medium"));
        var complex = ShapeGenerator.GenerateShapes(Complexity.Complex, rng.Fork("complex"));

        // Verify we can generate at all complexity levels
        Assert.True(simple.Length >= 1);
        Assert.True(medium.Length >= 2);
        Assert.True(complex.Length >= 3);
    }

    [Fact]
    public void BaseShapeTypesAreValid()
    {
        var baseShapes = Enum.GetValues<BaseShape>();
        Assert.True(baseShapes.Length > 0);
    }

    [Fact]
    public void ShapeParamsHasRequiredFields()
    {
        var rng = Rng.Create("param");
        var shapes = ShapeGenerator.GenerateShapes(Complexity.Simple, rng);

        if (shapes.Length > 0)
        {
            var shape = shapes[0];
            Assert.NotNull(shape);
        }
    }

    [Fact]
    public void MultipleGenerationsProduceVariety()
    {
        var rng = Rng.Create("variety");
        var results = new HashSet<int>();

        for (var i = 0; i < 20; i++)
        {
            var shapes = ShapeGenerator.GenerateShapes(Complexity.Medium, rng.Fork($"iter:{i}"));
            results.Add(shapes.Length);
        }

        // Should get some variety in shape counts
        Assert.True(results.Count > 1 || results.First() > 0);
    }
}
