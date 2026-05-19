namespace LexiconLang.Core;

public sealed class Context<TData>
{
    public IRng Rng { get; }
    public IReadOnlyList<string> Scope { get; }
    public IReadOnlySet<string> Tags { get; }
    public string? Locale { get; }
    public TData Data { get; }
    public IRegistry? Registry { get; }

    internal Context(
        IRng rng,
        IReadOnlyList<string> scope,
        IReadOnlySet<string> tags,
        TData data,
        string? locale = null,
        IRegistry? registry = null)
    {
        Rng = rng;
        Scope = scope;
        Tags = tags;
        Data = data;
        Locale = locale;
        Registry = registry;
    }

    public Context<TData> Child(string label, ContextOverrides<TData>? extra = null)
    {
        var tags = extra?.Tags is null ? Tags : new HashSet<string>(Tags.Concat(extra.Tags));
        return new Context<TData>(
            Rng.Fork(label),
            Scope.Concat([label]).ToArray(),
            tags,
            extra is { Data: not null } ? extra.Data : Data,
            extra?.Locale ?? Locale,
            extra?.Registry ?? Registry);
    }

    public Context<TData> WithTags(params string[] tags)
    {
        return new Context<TData>(
            Rng,
            Scope,
            new HashSet<string>(Tags.Concat(tags)),
            Data,
            Locale,
            Registry);
    }

    public Context<TNewData> WithData<TNewData>(TNewData data)
    {
        return new Context<TNewData>(Rng, Scope, Tags, data, Locale, Registry);
    }
}

public sealed record ContextOverrides<TData>(
    IEnumerable<string>? Tags = null,
    string? Locale = null,
    TData? Data = default,
    IRegistry? Registry = null);

public sealed record CreateContextOptions<TData>(
    object Seed,
    IEnumerable<string>? Tags = null,
    string? Locale = null,
    TData? Data = default,
    IRegistry? Registry = null);

public static class LexiconContext
{
    public static Context<TData> Create<TData>(CreateContextOptions<TData> options)
    {
        var rng = options.Seed switch
        {
            string str => Rng.Create(str),
            int i => Rng.Create(i),
            long l => Rng.Create(l),
            uint u => Rng.Create((ulong)u),
            ulong ul => Rng.Create(ul),
            RngState state => Rng.Create(state),
            _ => throw new ArgumentException("Unsupported seed type", nameof(options))
        };

        return new Context<TData>(
            rng,
            [],
            new HashSet<string>(options.Tags ?? []),
            options.Data!,
            options.Locale,
            options.Registry);
    }

    public static Context<object?> Create(string seed)
    {
        return Create(new CreateContextOptions<object?>(seed));
    }
}
