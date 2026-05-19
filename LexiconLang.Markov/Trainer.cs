namespace LexiconLang.Markov;

public sealed record TrainEntry(string Word, double Weight = 1.0);

public sealed record TrainOptions(
    int Order = 3,
    int MinLength = 3,
    int MaxLength = 14,
    int? RejectSubstringsOfLength = null,
    int? PruneBelow = null,
    bool Lowercase = true,
    Dictionary<string, object?>? Meta = null);

public static class Trainer
{
    public static MarkovModel Train(IEnumerable<TrainEntry> corpus, TrainOptions? options = null)
    {
        var opts = options ?? new TrainOptions();
        var entries = corpus
            .Select(e => new TrainEntry(
                opts.Lowercase ? e.Word.ToLowerInvariant() : e.Word,
                e.Weight))
            .Where(e => e.Word.Length > 0)
            .ToList();

        if (entries.Count == 0)
            throw new InvalidOperationException("markov.train: empty corpus");

        var transitions = new Dictionary<string, Dictionary<string, int>>();

        void Bump(string ctx, string ch, double weight)
        {
            if (!transitions.TryGetValue(ctx, out var row))
            {
                row = new Dictionary<string, int>();
                transitions[ctx] = row;
            }
            row[ch] = row.GetValueOrDefault(ch, 0) + (int)Math.Max(1, weight);
        }

        foreach (var (word, weight) in entries)
        {
            for (var i = 0; i < word.Length; i++)
            {
                var ctx = i == 0
                    ? MarkovTokens.Start
                    : word[Math.Max(0, i - opts.Order)..i];
                Bump(ctx, word[i].ToString(), weight);
            }

            var tailCtx = word[Math.Max(0, word.Length - opts.Order)..];
            Bump(tailCtx, MarkovTokens.End, weight);
        }

        // Prune low counts
        if (opts.PruneBelow > 1)
        {
            var pruneThreshold = opts.PruneBelow.Value;
            foreach (var (ctx, row) in transitions)
            {
                var toRemove = row.Where(kv => kv.Value < pruneThreshold).Select(kv => kv.Key).ToList();
                foreach (var ch in toRemove)
                    row.Remove(ch);
            }
            // Remove empty rows
            var emptyCtxs = transitions.Where(kv => kv.Value.Count == 0).Select(kv => kv.Key).ToList();
            foreach (var ctx in emptyCtxs)
                transitions.Remove(ctx);
        }

        // Build forbidden list
        HashSet<string>? forbidden = null;
        if (opts.RejectSubstringsOfLength.HasValue)
        {
            var minSub = opts.RejectSubstringsOfLength.Value;
            forbidden = new HashSet<string>();
            foreach (var (word, _) in entries)
            {
                if (word.Length >= minSub)
                    forbidden.Add(word);
            }
        }

        return new MarkovModel
        {
            Order = opts.Order,
            MinLength = opts.MinLength,
            MaxLength = opts.MaxLength,
            Transitions = transitions,
            Forbidden = forbidden,
            Meta = opts.Meta,
        };
    }
}