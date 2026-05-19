using LexiconLang.Core;

namespace LexiconLang.Glyphs;

public static class ShapeGenerator
{
    public static ShapeParams[] GenerateShapes(Complexity complexity, IRng rng)
    {
        var count = complexity switch
        {
            Complexity.Simple => rng.NextInt(1, 3),
            Complexity.Medium => rng.NextInt(2, 4),
            Complexity.Complex => rng.NextInt(3, 6),
            _ => 2
        };

        var shapes = new ShapeParams[count];
        var types = Enum.GetValues<BaseShape>();

        for (var i = 0; i < count; i++)
        {
            var type = types[rng.NextInt(0, types.Length)];
            shapes[i] = GenerateShape(type, rng);
        }

        return shapes;
    }

    public static ShapeParams GenerateShape(BaseShape type, IRng rng)
    {
        return type switch
        {
            BaseShape.Rect => new ShapeParams(BaseShape.Rect, new[]
            {
                rng.Next(), rng.Next(),          // x, y
                rng.NextRange(0.1, 0.8),         // w
                rng.NextRange(0.1, 0.8),         // h
            }),
            BaseShape.Circle => new ShapeParams(BaseShape.Circle, new[]
            {
                rng.Next(), rng.Next(),          // cx, cy
                rng.NextRange(0.05, 0.4),        // r
            }),
            BaseShape.Line => new ShapeParams(BaseShape.Line, new[]
            {
                rng.Next(), rng.Next(),          // x1, y1
                rng.Next(), rng.Next(),          // x2, y2
            }),
            BaseShape.Arc => new ShapeParams(BaseShape.Arc, new[]
            {
                rng.Next(), rng.Next(),          // cx, cy
                rng.NextRange(0.05, 0.4),        // r
                rng.NextRange(0, Math.PI * 2),   // startAngle
                rng.NextRange(0, Math.PI * 2),   // endAngle
            }),
            BaseShape.Polygon => GeneratePolygon(rng),
            _ => new ShapeParams(BaseShape.Circle, new[] { 0.5, 0.5, 0.2 }),
        };
    }

    private static ShapeParams GeneratePolygon(IRng rng)
    {
        var sides = rng.NextInt(3, 7);
        var cx = rng.NextRange(0.2, 0.8);
        var cy = rng.NextRange(0.2, 0.8);
        var radius = rng.NextRange(0.1, 0.35);
        var rotation = rng.NextRange(0, Math.PI * 2);

        var @params = new double[sides * 2 + 2];
        @params[0] = cx;
        @params[1] = cy;

        for (var i = 0; i < sides; i++)
        {
            var angle = rotation + (2 * Math.PI * i / sides);
            @params[2 + i * 2] = cx + Math.Cos(angle) * radius;
            @params[2 + i * 2 + 1] = cy + Math.Sin(angle) * radius;
        }

        return new ShapeParams(BaseShape.Polygon, @params);
    }
}
