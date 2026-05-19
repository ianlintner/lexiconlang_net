namespace LexiconLang.Core;

public readonly record struct GeneratorChoice<T, TContextData>(IGenerator<T, TContextData> Generator, double Weight = 1);
public readonly record struct CountSpec(int? Count = null, int? Min = null, int? Max = null);

public static class Combinators
{
    public static IGenerator<T, TContextData> WeightedList<T, TContextData>(IEnumerable<T> input, string? id = null)
    {
        var table = Sample.BuildAliasTable(Sample.NormalizeWeights(input));
        return new DelegateGenerator<T, TContextData>(
            id ?? Generators.NextAnonId("weightedList"),
            ctx => Sample.SampleAlias(table, ctx.Rng));
    }

    public static IGenerator<T, TContextData> WeightedList<T, TContextData>(IEnumerable<Weighted<T>> input, string? id = null)
    {
        var table = Sample.BuildAliasTable(Sample.NormalizeWeights(input));
        return new DelegateGenerator<T, TContextData>(
            id ?? Generators.NextAnonId("weightedList"),
            ctx => Sample.SampleAlias(table, ctx.Rng));
    }

    public static IGenerator<T, TContextData> WeightedList<T, TContextData>(IReadOnlyDictionary<T, double> input, string? id = null)
        where T : notnull
    {
        var table = Sample.BuildAliasTable(Sample.NormalizeWeights(input));
        return new DelegateGenerator<T, TContextData>(
            id ?? Generators.NextAnonId("weightedList"),
            ctx => Sample.SampleAlias(table, ctx.Rng));
    }

    public static IGenerator<T, TContextData> OneOf<T, TContextData>(params T[] values)
    {
        return WeightedList<T, TContextData>(values);
    }

    public static IGenerator<T, TContextData> PickOf<T, TContextData>(params GeneratorChoice<T, TContextData>[] choices)
    {
        var normalized = choices.Select((choice, index) =>
            new Weighted<IGenerator<T, TContextData>>(choice.Generator, choice.Weight > 0 ? choice.Weight : 1)).ToArray();
        var table = Sample.BuildAliasTable(normalized);

        return new DelegateGenerator<T, TContextData>(
            Generators.NextAnonId("pickOf"),
            ctx =>
            {
                var chosen = Sample.SampleAlias(table, ctx.Rng);
                return chosen.Generate(ctx.Child($"choice:{chosen.Id}"));
            });
    }

    public static IGenerator<IReadOnlyList<T>, TContextData> Repeat<T, TContextData>(
        IGenerator<T, TContextData> generator,
        CountSpec count)
    {
        return new DelegateGenerator<IReadOnlyList<T>, TContextData>(
            Generators.NextAnonId("repeat"),
            ctx =>
            {
                var n = count.Count ?? ctx.Rng.Fork("count").NextInt(count.Min ?? 0, (count.Max ?? 0) + 1);
                var output = new List<T>(n);
                for (var i = 0; i < n; i++)
                {
                    output.Add(generator.Generate(ctx.Child($"i:{i}")));
                }

                return output;
            });
    }

    public static IGenerator<TOutput, TContextData> Compose<TOutput, TContextData>(
        string id,
        Func<Context<TContextData>, TOutput> factory)
    {
        return new DelegateGenerator<TOutput, TContextData>(id, factory);
    }

    public static IGenerator<TOut, TContextData> Map<TIn, TOut, TContextData>(
        IGenerator<TIn, TContextData> generator,
        Func<TIn, Context<TContextData>, TOut> map)
    {
        return new DelegateGenerator<TOut, TContextData>(
            Generators.NextAnonId("map"),
            ctx => map(generator.Generate(ctx), ctx));
    }

    public static IGenerator<TOut, TContextData> Chain<TIn, TOut, TContextData>(
        IGenerator<TIn, TContextData> generator,
        Func<TIn, Context<TContextData>, IGenerator<TOut, TContextData>> next)
    {
        return new DelegateGenerator<TOut, TContextData>(
            Generators.NextAnonId("chain"),
            ctx =>
            {
                var a = generator.Generate(ctx.Child("a"));
                var b = next(a, ctx);
                return b.Generate(ctx.Child("b"));
            });
    }

    public static IGenerator<T, TContextData> Constant<T, TContextData>(T value)
    {
        return new DelegateGenerator<T, TContextData>(Generators.NextAnonId("constant"), _ => value);
    }

    public static IGenerator<int, TContextData> IntRange<TContextData>(int min, int maxInclusive)
    {
        return new DelegateGenerator<int, TContextData>(
            Generators.NextAnonId("intRange"),
            ctx => ctx.Rng.NextInt(min, maxInclusive + 1));
    }

    public static IGenerator<double, TContextData> FloatRange<TContextData>(double min, double max)
    {
        return new DelegateGenerator<double, TContextData>(
            Generators.NextAnonId("floatRange"),
            ctx => ctx.Rng.NextRange(min, max));
    }
}
