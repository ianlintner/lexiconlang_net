namespace LexiconLang.Cli;

public sealed class ParsedArgs
{
    public string? Command { get; set; }
    public List<string> Positional { get; set; } = new();
    public Dictionary<string, object> Options { get; set; } = new();
}

public static class ArgParser
{
    public static ParsedArgs Parse(string[] argv)
    {
        var args = new ParsedArgs();
        var i = 0;

        while (i < argv.Length)
        {
            var a = argv[i];

            if (i == 0 && !a.StartsWith('-'))
            {
                args.Command = a;
                i++;
                continue;
            }

            if (a.StartsWith("--"))
            {
                var body = a[2..];
                var eq = body.IndexOf('=');
                if (eq >= 0)
                {
                    args.Options[body[..eq]] = body[(eq + 1)..];
                    i++;
                    continue;
                }

                if (body.StartsWith("no-"))
                {
                    args.Options[body[3..]] = false;
                    i++;
                    continue;
                }

                var next = i + 1 < argv.Length ? argv[i + 1] : null;
                if (next != null && !next.StartsWith('-'))
                {
                    args.Options[body] = next;
                    i += 2;
                }
                else
                {
                    args.Options[body] = true;
                    i++;
                }
                continue;
            }

            if (a.StartsWith('-'))
            {
                args.Options[a[1..]] = true;
                i++;
                continue;
            }

            args.Positional.Add(a);
            i++;
        }

        return args;
    }

    public static string RequireOption(ParsedArgs args, string key)
    {
        if (!args.Options.TryGetValue(key, out var v) || v is not string s)
            throw new InvalidOperationException($"Missing required --{key}");
        return s;
    }

    public static int IntOption(ParsedArgs args, string key, int fallback)
    {
        if (args.Options.TryGetValue(key, out var v) && v is string s && int.TryParse(s, out var n))
            return n;
        return fallback;
    }

    public static bool BoolOption(ParsedArgs args, string key, bool fallback = false)
    {
        if (args.Options.TryGetValue(key, out var v))
            return v is bool b ? b : v is string s && s.Equals("true", StringComparison.OrdinalIgnoreCase);
        return fallback;
    }
}
