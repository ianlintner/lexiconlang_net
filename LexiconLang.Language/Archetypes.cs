namespace LexiconLang.Language;

public static class Archetypes
{
    private static GlyphClass V(string id, params string[] glyphs) => new(id, glyphs);

    public static readonly GlyphSystem Flowing = new(
        classes: new[]
        {
            V("vowel", "a", "e", "i", "o", "u"),
            V("liquid", "l", "r", "n", "m", "w", "y"),
        },
        syllables: new[]
        {
            new[] { "liquid", "vowel" },
            new[] { "liquid", "vowel", "liquid" },
            new[] { "vowel", "liquid" },
        },
        wordShapes: new[] { "2", "3", "2-3" },
        joiner: ""
    );

    public static readonly GlyphSystem Guttural = new(
        classes: new[]
        {
            V("vowel", "a", "o", "u"),
            V("guttural", "k", "g", "d", "t", "p", "b"),
        },
        syllables: new[]
        {
            new[] { "guttural", "vowel" },
            new[] { "guttural", "vowel", "guttural" },
        },
        wordShapes: new[] { "1", "2", "1-2" },
        joiner: ""
    );

    public static readonly GlyphSystem Clipped = new(
        classes: new[]
        {
            V("vowel", "a", "i", "u"),
            V("stop", "k", "t", "p", "s", "sh", "ch"),
        },
        syllables: new[]
        {
            new[] { "stop", "vowel" },
            new[] { "stop", "vowel", "stop" },
        },
        wordShapes: new[] { "1", "2", "1-2" },
        joiner: ""
    );

    public static readonly GlyphSystem Sibilant = new(
        classes: new[]
        {
            V("vowel", "a", "e", "i", "o", "u"),
            V("sibilant", "s", "sh", "z", "zh", "ts", "ch", "j"),
        },
        syllables: new[]
        {
            new[] { "sibilant", "vowel" },
            new[] { "vowel", "sibilant" },
            new[] { "sibilant", "vowel", "sibilant" },
        },
        wordShapes: new[] { "2", "3", "2-3" },
        joiner: ""
    );

    public static readonly GlyphSystem Resonant = new(
        classes: new[]
        {
            V("vowel", "a", "e", "i", "o"),
            V("resonant", "l", "r", "m", "n", "ng"),
        },
        syllables: new[]
        {
            new[] { "resonant", "vowel" },
            new[] { "resonant", "vowel", "resonant" },
        },
        wordShapes: new[] { "2", "3", "2-4" },
        joiner: ""
    );
}