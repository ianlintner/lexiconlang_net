using LexiconLang.Core;

namespace LexiconLang.Markov;

public sealed record SampleOptions(
    int MaxAttempts = 200,
    int? MinLength = null,
    int? MaxLength = null,
    bool Capitalize = true);

public static class Sampler
{
    public static string Sample(MarkovModel model, IRng rng, SampleOptions? options = null)
    {
        var opts = options ?? new SampleOptions();
        var maxLen = opts.MaxLength ?? model.MaxLength;
        var minLen = opts.MinLength ?? model.MinLength;
        var cap = opts.Capitalize;

        for (var attempt = 0; attempt < opts.MaxAttempts; attempt++)
        {
            var outStr = "";
            var ctx = MarkovTokens.Start;
            var fail = false;

            for (var i = 0; i < maxLen + 1; i++)
            {
                if (!model.Transitions.TryGetValue(ctx, out var row))
                {
                    // Back off to shorter context
                    Dictionary<string, int>? backoffRow = null;
                    var backoff = ctx.Length > 1 ? ctx[1..] : "";
                    while (backoff.Length > 0)
                    {
                        if (model.Transitions.TryGetValue(backoff, out backoffRow))
                            break;
                        backoff = backoff.Length > 1 ? backoff[1..] : "";
                    }

                    if (backoffRow == null)
                    {
                        fail = true;
                        break;
                    }

                    var ch = PickWeighted(rng, backoffRow);
                    if (ch == null || ch == MarkovTokens.End)
                        break;

                    outStr += ch;
                    ctx = NextContext(outStr, model.Order);
                    continue;
                }

                var next = PickWeighted(rng, row);
                if (next == null)
                {
                    fail = true;
                    break;
                }

                if (next == MarkovTokens.End)
                {
                    if (outStr.Length >= minLen) break;

                    // Force a non-END choice
                    var alt = new Dictionary<string, int>();
                    foreach (var (k, v) in row)
                    {
                        if (k != MarkovTokens.End)
                            alt[k] = v;
                    }

                    if (alt.Count == 0)
                    {
                        fail = true;
                        break;
                    }

                    var altCh = PickWeighted(rng, alt);
                    if (altCh == null)
                    {
                        fail = true;
                        break;
                    }

                    outStr += altCh;
                    ctx = NextContext(outStr, model.Order);
                    continue;
                }

                outStr += next;
                ctx = NextContext(outStr, model.Order);
            }

            if (!fail && outStr.Length >= minLen && outStr.Length <= maxLen)
            {
                if (ViolatesForbidden(outStr, model.Forbidden))
                    continue;

                return cap && outStr.Length > 0
                    ? char.ToUpperInvariant(outStr[0]) + outStr[1..]
                    : outStr;
            }
        }

        throw new InvalidOperationException($"markov.sample: failed after {opts.MaxAttempts} attempts");
    }

    public static IGenerator<string, TContextData> AsGenerator<TContextData>(
        MarkovModel model, SampleOptions? options = null)
    {
        var opts = options ?? new SampleOptions();
        return new DelegateGen<TContextData>(model, opts);
    }

    private sealed class DelegateGen<TContextData> : IGenerator<string, TContextData>
    {
        private readonly MarkovModel _model;
        private readonly SampleOptions _opts;
        private static int _counter;
        private readonly string _id = $"markov:{Interlocked.Increment(ref _counter)}";

        public DelegateGen(MarkovModel model, SampleOptions opts)
        {
            _model = model;
            _opts = opts;
        }

        public string Id => _id;
        public string Generate(Context<TContextData> context) => Sample(_model, context.Rng, _opts);
    }

    private static string? PickWeighted(IRng rng, Dictionary<string, int> choices)
    {
        var total = choices.Values.Sum();
        if (total <= 0) return null;

        var r = rng.Next() * total;
        foreach (var (k, v) in choices)
        {
            r -= v;
            if (r <= 0) return k;
        }

        // Floating point fallback
        var keys = choices.Keys.ToArray();
        return keys.Length > 0 ? keys[^1] : null;
    }

    private static string NextContext(string buf, int order)
    {
        return buf[Math.Max(0, buf.Length - order)..];
    }

    private static bool ViolatesForbidden(string s, HashSet<string>? forbidden)
    {
        if (forbidden == null) return false;
        foreach (var f in forbidden)
        {
            if (s.Length >= f.Length && s.Contains(f, StringComparison.Ordinal))
                return true;
        }
        return false;
    }
}