namespace LexiconLang.Language;

public static class ConstraintMatcher
{
    /// <summary>
    /// Checks if a constraint pattern matches a class sequence.
    /// Supports "*" wildcard matching any single class.
    /// </summary>
    public static bool Matches(string[] pattern, string[] sequence)
    {
        if (pattern.Length != sequence.Length)
            return false;

        for (var i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] != "*" && pattern[i] != sequence[i])
                return false;
        }

        return true;
    }
}
