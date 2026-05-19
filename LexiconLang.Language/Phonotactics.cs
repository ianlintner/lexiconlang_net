using LexiconLang.Core;

namespace LexiconLang.Language;

public static class Phonotactics
{
    public static string GenerateWord<TContextData>(GlyphSystem glyphs, Context<TContextData> ctx)
    {
        // 1. Pick word shape
        var shapeCtx = ctx.Child("shape");
        var shapeStr = PickWeighted(glyphs.WordShapes, shapeCtx.Rng);
        var syllableCount = ParseWordShape(shapeStr, shapeCtx.Rng);

        // 2. Generate syllables
        var syllables = new List<string>();
        for (var i = 0; i < syllableCount; i++)
        {
            var syllCtx = ctx.Child($"syl:{i}");
            var template = PickWeighted(glyphs.Syllables, syllCtx.Rng);
            var classes = template;
            var syllableGlyphs = GenerateSyllable(glyphs, classes, syllCtx);
            syllables.Add(syllableGlyphs);
        }

        // 3. Join
        var word = string.Join(glyphs.Joiner, syllables);

        // 4. Capitalize
        return Capitalize(word);
    }

    private static int ParseWordShape(string shape, IRng rng)
    {
        if (shape.Contains('-'))
        {
            var parts = shape.Split('-');
            if (parts.Length == 2 && int.TryParse(parts[0], out var lo) && int.TryParse(parts[1], out var hi))
                return rng.NextInt(lo, hi + 1);
        }
        return int.TryParse(shape, out var n) ? n : 2;
    }

    private static string GenerateSyllable<TContextData>(GlyphSystem glyphs, string[] classes, Context<TContextData> ctx)
    {
        var classMap = glyphs.Classes.ToDictionary(c => c.Id);
        var glyphSlots = new List<string>();

        for (var slot = 0; slot < classes.Length; slot++)
        {
            var className = classes[slot];
            if (!classMap.TryGetValue(className, out var glyphClass))
                continue;

            var glyph = PickGlyph(glyphClass.Glyphs, ctx.Child($"slot:{slot}").Rng);

            // Constraint checking with retries
            var retries = 0;
            while (retries < 8 && ViolatesConstraints(glyphs, glyphSlots.Append(glyph).ToArray(), classes, slot))
            {
                glyph = PickGlyph(glyphClass.Glyphs, ctx.Child($"slot:{slot}:retry:{retries}").Rng);
                retries++;
            }

            glyphSlots.Add(glyph);
        }

        return string.Join("", glyphSlots);
    }

    private static bool ViolatesConstraints(GlyphSystem glyphs, string[] glyphsSoFar, string[] classes, int currentSlot)
    {
        if (glyphs.Constraints == null || glyphsSoFar.Length < 2)
            return false;

        foreach (var constraint in glyphs.Constraints)
        {
            var patternLen = constraint.Pattern.Length;
            var startIdx = Math.Max(0, currentSlot + 1 - patternLen);

            for (var i = startIdx; i <= currentSlot - patternLen + 1; i++)
            {
                var classSeq = classes[i..(i + patternLen)];
                if (ConstraintMatcher.Matches(constraint.Pattern, classSeq))
                {
                    var glyphSeq = glyphsSoFar[i..(i + patternLen)];
                    if (glyphSeq.Length == patternLen)
                    {
                        if (constraint.Rule is ConstraintRule.Forbid)
                            return true;

                        if (constraint.Rule is ConstraintRule.MaxOccurrences maxOcc)
                        {
                            var count = glyphSeq.Count(g => g == glyphSeq[0]);
                            if (count > maxOcc.Max)
                                return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private static string PickGlyph(string[] glyphs, IRng rng)
    {
        return glyphs[rng.NextInt(0, glyphs.Length)];
    }

    private static T PickWeighted<T>(T[] items, IRng rng)
    {
        return items[rng.NextInt(0, items.Length)];
    }

    private static string Capitalize(string word)
    {
        if (string.IsNullOrEmpty(word)) return word;
        return char.ToUpperInvariant(word[0]) + word[1..];
    }
}
