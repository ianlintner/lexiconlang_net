namespace LexiconLang.Glyphs;

public enum BaseShape { Rect, Circle, Line, Arc, Polygon }

public enum Complexity { Simple, Medium, Complex }

public enum MappingStrategy { Phoneme, Morpheme, Holistic }

public enum RenderFormat { Svg, Unicode, Canvas }

public sealed record ShapeParams(BaseShape Type, double[] Parameters);

public sealed record RenderParams(
    int Size = 64,
    double StrokeWidth = 1.5,
    string[]? Palette = null,
    string? Fallback = null);

public abstract record DrawInstruction
{
    public sealed record MoveTo(double X, double Y) : DrawInstruction;
    public sealed record LineTo(double X, double Y) : DrawInstruction;
    public sealed record Arc(double X, double Y, double Radius, double StartAngle, double EndAngle) : DrawInstruction;
    public sealed record Rect(double X, double Y, double W, double H) : DrawInstruction;
    public sealed record Fill(string Color) : DrawInstruction;
    public sealed record Stroke(string Color, double Width) : DrawInstruction;
    public sealed record BeginPath : DrawInstruction;
    public sealed record ClosePath : DrawInstruction;
}

public sealed record VisualGlyphSystem(
    string Name,
    MappingStrategy Strategy,
    Complexity Complexity,
    RenderFormat Format,
    string[]? Palette = null);

public sealed record Glyph(
    string? Id = null,
    string? Meaning = null,
    string? Svg = null,
    DrawInstruction[]? CanvasInstructions = null,
    string? Unicode = null);

public sealed record GlyphSet(
    Glyph[]? Phonetic = null,
    Glyph[]? Conceptual = null,
    Glyph? Holistic = null);
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lexiconlang_net
{
    public class Types
    {
        
    }
}