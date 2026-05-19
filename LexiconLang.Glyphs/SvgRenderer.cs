using System.Text;

namespace LexiconLang.Glyphs;

public static class SvgRenderer
{
    public static string RenderToSvg(ShapeParams[] shapes, RenderParams? renderParams = null)
    {
        var p = renderParams ?? new RenderParams();
        var size = p.Size;
        var sw = p.StrokeWidth;
        var palette = p.Palette ?? new[] { "#333333", "#666666", "#999999", "#222222" };
        var sb = new StringBuilder();

        sb.Append($"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {size} {size}\">");

        for (var i = 0; i < shapes.Length; i++)
        {
            var color = palette[i % palette.Length];
            var shape = shapes[i];
            sb.Append(RenderShape(shape, size, sw, color));
        }

        sb.Append("</svg>");
        return sb.ToString();
    }

    private static string RenderShape(ShapeParams shape, int size, double sw, string color)
    {
        var scaled = ScaleParams(shape.Parameters, size);
        var stroke = $"stroke=\"{color}\" stroke-width=\"{sw:F1}\" fill=\"none\"";

        return shape.Type switch
        {
            BaseShape.Rect => $"<rect x=\"{scaled[0]:F1}\" y=\"{scaled[1]:F1}\" width=\"{scaled[2]:F1}\" height=\"{scaled[3]:F1}\" {stroke}/>",
            BaseShape.Circle => $"<circle cx=\"{scaled[0]:F1}\" cy=\"{scaled[1]:F1}\" r=\"{scaled[2]:F1}\" {stroke}/>",
            BaseShape.Line => $"<line x1=\"{scaled[0]:F1}\" y1=\"{scaled[1]:F1}\" x2=\"{scaled[2]:F1}\" y2=\"{scaled[3]:F1}\" {stroke}/>",
            BaseShape.Arc => RenderArcSvg(scaled, stroke),
            BaseShape.Polygon => RenderPolygonSvg(scaled, stroke),
            _ => ""
        };
    }

    private static string RenderArcSvg(double[] p, string stroke)
    {
        // Arc parameters: cx, cy, r, startAngle, endAngle
        var cx = p[0]; var cy = p[1]; var r = p[2];
        var startAngle = p[3]; var endAngle = p[4];

        var x1 = cx + r * Math.Cos(startAngle);
        var y1 = cy + r * Math.Sin(startAngle);
        var x2 = cx + r * Math.Cos(endAngle);
        var y2 = cy + r * Math.Sin(endAngle);

        var largeArc = Math.Abs(endAngle - startAngle) > Math.PI ? 1 : 0;

        return $"<path d=\"M{x1:F1} {y1:F1} A{r:F1} {r:F1} 0 {largeArc} 1 {x2:F1} {y2:F1}\" {stroke}/>";
    }

    private static string RenderPolygonSvg(double[] p, string stroke)
    {
        // Polygon: cx, cy, then x,y pairs
        var points = new StringBuilder();
        for (var i = 2; i < p.Length; i += 2)
        {
            if (points.Length > 0) points.Append(' ');
            points.Append($"{p[i]:F1},{p[i + 1]:F1}");
        }
        return $"<polygon points=\"{points}\" {stroke}/>";
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lexiconlang_net
{
    public class SvgRenderer
    {
        
    }
}