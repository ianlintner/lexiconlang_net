using LexiconLang.Core;
using LexiconLang.Language;

namespace LexiconLang.Glyphs;

public static class GlyphGenerator
{
    public static GlyphSet GlyphsFor<TContextData>(TranslatedName name, VisualGlyphSystem system, Context<TContextData> ctx)
    {
        return system.Strategy switch
        {
            MappingStrategy.Phoneme => GeneratePhonemeGlyphs(name, system, ctx),
            MappingStrategy.Morpheme => GenerateMorphemeGlyphs(name, system, ctx),
            MappingStrategy.Holistic => GenerateHolisticGlyph(name, system, ctx),
            _ => new GlyphSet(),
        };
    }

    private static GlyphSet GeneratePhonemeGlyphs<TContextData>(TranslatedName name, VisualGlyphSystem system, Context<TContextData> ctx)
    {
        var pairs = SplitByPairs(name.Form);
        var glyphs = new List<Glyph>();

        for (var i = 0; i < pairs.Length; i++)
        {
            var glyphCtx = ctx.Child($"phoneme:{i}");
            var shapes = ShapeGenerator.GenerateShapes(system.Complexity, glyphCtx.Rng);
            var glyph = CreateGlyph(pairs[i], shapes, system);
            glyphs.Add(glyph);
        }

        return new GlyphSet(Phonetic: glyphs.ToArray());
    }

    private static GlyphSet GenerateMorphemeGlyphs<TContextData>(TranslatedName name, VisualGlyphSystem system, Context<TContextData> ctx)
    {
        var parts = name.Translation?.Split('-') ?? name.Form.Split('-');
        if (parts.Length == 0) parts = new[] { name.Form };

        var glyphs = new List<Glyph>();

        for (var i = 0; i < parts.Length; i++)
        {
            var glyphCtx = ctx.Child($"morpheme:{i}");
            var shapes = ShapeGenerator.GenerateShapes(system.Complexity, glyphCtx.Rng);
            var glyph = CreateGlyph(parts[i].Trim(), shapes, system);
            glyphs.Add(glyph);
        }

        return new GlyphSet(Conceptual: glyphs.ToArray());
    }

    private static GlyphSet GenerateHolisticGlyph<TContextData>(TranslatedName name, VisualGlyphSystem system, Context<TContextData> ctx)
    {
        var shapes = ShapeGenerator.GenerateShapes(system.Complexity, ctx.Rng);
        var glyph = CreateGlyph(name.Form, shapes, system);
        return new GlyphSet(Holistic: glyph);
    }

    private static Glyph CreateGlyph(string meaning, ShapeParams[] shapes, VisualGlyphSystem system)
    {
        var glyph = new Glyph
        {
            Meaning = meaning,
        };

        switch (system.Format)
        {
            case RenderFormat.Svg:
                glyph = glyph with
                {
                    Svg = SvgRenderer.RenderToSvg(shapes, new RenderParams(Palette: system.Palette))
                };
                break;
            case RenderFormat.Canvas:
                glyph = glyph with
                {
                    CanvasInstructions = CanvasRenderer.RenderToCanvas(shapes, new RenderParams(Palette: system.Palette))
                };
                break;
            case RenderFormat.Unicode:
                glyph = glyph with
                {
                    Unicode = UnicodeRenderer.RenderToUnicode(meaning)
                };
                break;
        }

        return glyph;
    }

    private static string[] SplitByPairs(string input)
    {
        if (input.Length <= 2)
            return new[] { input };

        var result = new List<string>();
        for (var i = 0; i < input.Length; i += 2)
        {
            result.Add(i + 1 < input.Length ? input[i..(i + 2)] : input[i..]);
        }
        return result.ToArray();
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lexiconlang_net
{
    public class GlyphGenerator
    {
        
    }
}