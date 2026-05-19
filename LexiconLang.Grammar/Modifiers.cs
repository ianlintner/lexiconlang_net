namespace LexiconLang.Grammar;

/// <summary>
/// Text modifier function: transforms input string with optional arguments.
/// </summary>
public delegate string Modifier(string input, params string[] args);

public static class BuiltinModifiers
{
    public static readonly Dictionary<string, Modifier> All = new()
    {
        // Capitalization
        ["cap"] = CapitalizeMod,
        ["capitalize"] = CapitalizeMod,
        ["upper"] = UpperMod,
        ["uppercase"] = UpperMod,
        ["lower"] = LowerMod,
        ["lowercase"] = LowerMod,
        ["title"] = TitleMod,
        ["titlecase"] = TitleMod,

        // Articles
        ["a"] = ArticleMod,
        ["an"] = ArticleMod,
        ["article"] = ArticleMod,

        // English morphology (best-effort)
        ["s"] = PluralizeMod,
        ["plural"] = PluralizeMod,
        ["ed"] = PastTenseMod,
        ["past"] = PastTenseMod,
        ["possessive"] = PossessiveMod,

        // String utilities
        ["trim"] = TrimMod,
        ["reverse"] = ReverseMod,
        ["replace"] = ReplaceMod,
    };

    public static string CapitalizeMod(string input, params string[] args)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToUpper(input[0]) + input[1..];
    }

    public static string UpperMod(string input, params string[] args) => input.ToUpperInvariant();

    public static string LowerMod(string input, params string[] args) => input.ToLowerInvariant();

    public static string TitleMod(string input, params string[] args)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var parts = input.Split(' ');
        for (var i = 0; i < parts.Length; i++)
        {
            if (parts[i].Length > 0)
                parts[i] = char.ToUpper(parts[i][0]) + parts[i][1..].ToLowerInvariant();
        }
        return string.Join(" ", parts);
    }

    public static string ArticleMod(string input, params string[] args)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var first = char.ToLowerInvariant(input[0]);
        var article = first is 'a' or 'e' or 'i' or 'o' or 'u' ? "an" : "a";
        return $"{article} {input}";
    }

    public static string PluralizeMod(string input, params string[] args)
    {
        if (string.IsNullOrEmpty(input)) return input;
        // Best-effort English pluralization heuristics
        var lower = input.ToLowerInvariant();
        if (lower.EndsWith("s") || lower.EndsWith("sh") || lower.EndsWith("ch") || lower.EndsWith("x") || lower.EndsWith("z"))
            return input + "es";
        if (lower.EndsWith("y") && input.Length > 1 && !IsVowel(input[^2]))
            return input[..^1] + "ies";
        if (lower.EndsWith("f"))
            return input[..^1] + "ves";
        if (lower.EndsWith("fe"))
            return input[..^2] + "ves";
        if (lower.EndsWith("man") && input.Length > 3)
            return input[..^2] + "en";
        return input + "s";
    }

    public static string PastTenseMod(string input, params string[] args)
    {
        if (string.IsNullOrEmpty(input)) return input;
        // Best-effort English past tense heuristics
        var lower = input.ToLowerInvariant();
        if (lower.EndsWith("e"))
            return input + "d";
        if (lower.EndsWith("y") && input.Length > 1 && !IsVowel(input[^2]))
            return input[..^1] + "ied";
        // Double final consonant for CVC pattern
        if (input.Length >= 3 && !IsVowel(input[^3]) && IsVowel(input[^2]) && !IsVowel(input[^1]) && input[^1] != 'w' && input[^1] != 'x' && input[^1] != 'y')
            return input + input[^1] + "ed";
        return input + "ed";
    }

    public static string PossessiveMod(string input, params string[] args)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return input.EndsWith('s') ? input + "'" : input + "'s";
    }

    public static string TrimMod(string input, params string[] args) => input.Trim();

    public static string ReverseMod(string input, params string[] args)
    {
        var chars = input.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    public static string ReplaceMod(string input, params string[] args)
    {
        if (args.Length < 2) return input;
        return input.Replace(args[0], args[1]);
    }

    private static bool IsVowel(char c)
    {
        c = char.ToLowerInvariant(c);
        return c is 'a' or 'e' or 'i' or 'o' or 'u';
    }
}
