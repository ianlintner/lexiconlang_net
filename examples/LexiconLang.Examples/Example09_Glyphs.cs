using System;
using LexiconLang.Core;
using LexiconLang.Language;
using LexiconLang.Fantasy;
using LexiconLang.Glyphs;

namespace LexiconLang.Examples;

public static class Example09_Glyphs
{
    public static void Run()
    {
        var ctx = LexiconContext.Create("glyph-demo");

        var dwarfName = Templates.GenerateName(FantasyCultures.Dwarvish, "given", ctx.Child("dwarf"));
        
        var visualSystem = new VisualGlyphSystem(
            Name: "dwarvish-runes",
            Strategy: MappingStrategy.Phoneme,
            Complexity: Complexity.Medium,
            Format: RenderFormat.Svg,
            Palette: new[] { "#333", "#666" }
        );
        
        var dwarfGlyphs = GlyphGenerator.GlyphsFor(dwarfName, visualSystem, ctx.Child("dwarf"));

        Console.WriteLine($"Dwarvish — {dwarfName.Form} ({dwarfName.Translation})");
        Console.WriteLine($"  {dwarfGlyphs.Phonetic?.Length} runes, one per phoneme pair");
        if (dwarfGlyphs.Phonetic != null)
        {
            for (var i = 0; i < dwarfGlyphs.Phonetic.Length; i++)
            {
                var svg = dwarfGlyphs.Phonetic[i].Svg ?? "";
                svg = svg.Substring(0, Math.Min(70, svg.Length));
                Console.WriteLine($"  [{i}] {svg}…");
            }
        }
    }
}
