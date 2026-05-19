namespace LexiconLang.Glyphs;

public static class CanvasRenderer
{
    public static DrawInstruction[] RenderToCanvas(ShapeParams[] shapes, RenderParams? renderParams = null)
    {
        var p = renderParams ?? new RenderParams();
        var size = p.Size;
        var sw = p.StrokeWidth;
        var palette = p.Palette ?? new[] { "#333333", "#666666", "#999999", "#222222" };

        var instructions = new List<DrawInstruction>();
        instructions.Add(new DrawInstruction.BeginPath());

        for (var i = 0; i < shapes.Length; i++)
        {
            var color = palette[i % palette.Length];
            var shape = shapes[i];
            var scaled = ScaleParams(shape.Parameters, size);

            switch (shape.Type)
            {
                case BaseShape.Rect:
                    instructions.Add(new DrawInstruction.Rect(scaled[0], scaled[1], scaled[2], scaled[3]));
                    break;
                case BaseShape.Circle:
                    instructions.Add(new DrawInstruction.MoveTo(scaled[0] + scaled[2], scaled[1]));
                    instructions.Add(new DrawInstruction.Arc(scaled[0], scaled[1], scaled[2], 0, Math.PI * 2));
                    break;
                case BaseShape.Line:
                    instructions.Add(new DrawInstruction.MoveTo(scaled[0], scaled[1]));
                    instructions.Add(new DrawInstruction.LineTo(scaled[2], scaled[3]));
                    break;
                case BaseShape.Arc:
                    var cx = scaled[0]; var cy = scaled[1]; var r = scaled[2];
                    var startAngle = shape.Parameters[3];
                    var x1 = cx + r * Math.Cos(startAngle);
                    var y1 = cy + r * Math.Sin(startAngle);
                    instructions.Add(new DrawInstruction.MoveTo(x1, y1));
                    instructions.Add(new DrawInstruction.Arc(cx, cy, r, startAngle, shape.Parameters[4]));
                    break;
                case BaseShape.Polygon:
                    instructions.Add(new DrawInstruction.MoveTo(scaled[2], scaled[3]));
                    for (var j = 4; j < scaled.Length; j += 2)
                    {
                        instructions.Add(new DrawInstruction.LineTo(scaled[j], scaled[j + 1]));
                    }
                    instructions.Add(new DrawInstruction.ClosePath());
                    break;
            }
        }

        instructions.Add(new DrawInstruction.Stroke(palette[0], sw));
        return instructions.ToArray();
    }

    private static double[] ScaleParams(double[] @params, int size)
    {
        var scaled = new double[@params.Length];
        for (var i = 0; i < @params.Length; i++)
        {
            scaled[i] = @params[i] * size;
        }
        return scaled;
    }
}
