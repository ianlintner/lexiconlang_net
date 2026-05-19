namespace LexiconLang.Core;

public readonly record struct Weighted<T>(T Value, double Weight);

public sealed class AliasTable<T>
{
    public IReadOnlyList<T> Values { get; init; } = [];
    public double[] Probabilities { get; init; } = [];
    public int[] Alias { get; init; } = [];
}

public static class Sample
{
    public static IReadOnlyList<Weighted<T>> NormalizeWeights<T>(IEnumerable<T> input)
    {
        return input.Select(item => new Weighted<T>(item, 1)).ToArray();
    }

    public static IReadOnlyList<Weighted<T>> NormalizeWeights<T>(IEnumerable<Weighted<T>> input)
    {
        return input.ToArray();
    }

    public static IReadOnlyList<Weighted<T>> NormalizeWeights<T>(IReadOnlyDictionary<T, double> input) where T : notnull
    {
        return input
            .Where(entry => double.IsFinite(entry.Value) && entry.Value > 0)
            .Select(entry => new Weighted<T>(entry.Key, entry.Value))
            .ToArray();
    }

    public static AliasTable<T> BuildAliasTable<T>(IEnumerable<Weighted<T>> input)
    {
        var weighted = input.ToArray();
        if (weighted.Length == 0) throw new InvalidOperationException("BuildAliasTable: empty input");

        var n = weighted.Length;
        var values = weighted.Select(x => x.Value).ToArray();
        var sum = weighted.Sum(x => x.Weight);
        if (sum <= 0) throw new InvalidOperationException("BuildAliasTable: total weight must be > 0");

        var prob = new double[n];
        var alias = new int[n];
        var scaled = weighted.Select(x => (x.Weight * n) / sum).ToArray();

        var small = new Stack<int>();
        var large = new Stack<int>();
        for (var i = 0; i < n; i++)
        {
            if (scaled[i] < 1) small.Push(i);
            else large.Push(i);
        }

        while (small.Count > 0 && large.Count > 0)
        {
            var s = small.Pop();
            var l = large.Pop();
            prob[s] = scaled[s];
            alias[s] = l;
            scaled[l] = scaled[l] + scaled[s] - 1;
            if (scaled[l] < 1) small.Push(l);
            else large.Push(l);
        }

        while (large.Count > 0)
        {
            var l = large.Pop();
            prob[l] = 1;
            alias[l] = l;
        }

        while (small.Count > 0)
        {
            var s = small.Pop();
            prob[s] = 1;
            alias[s] = s;
        }

        return new AliasTable<T> { Values = values, Probabilities = prob, Alias = alias };
    }

    public static T SampleAlias<T>(AliasTable<T> table, IRng rng)
    {
        var n = table.Values.Count;
        var i = rng.NextInt(0, n);
        var r = rng.Next();
        var idx = r < table.Probabilities[i] ? i : table.Alias[i];
        return table.Values[idx];
    }
}
