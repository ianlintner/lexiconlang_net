using System.Threading;

namespace LexiconLang.Core;

public interface IGenerator<T, TContextData>
{
    string Id { get; }
    T Generate(Context<TContextData> context);
}

public readonly record struct GeneratorInfo(string Id, string? Description = null, IReadOnlyList<string>? Tags = null);

public static class Generators
{
    private static int _anonCounter;

    public static string NextAnonId(string prefix = "anon")
    {
        return $"{prefix}:{Interlocked.Increment(ref _anonCounter)}";
    }

    public static IGenerator<T, TContextData> AsGenerator<T, TContextData>(
        object generatorLike,
        string fallbackId = "anon")
    {
        if (generatorLike is IGenerator<T, TContextData> generator)
        {
            return generator;
        }

        if (generatorLike is Func<Context<TContextData>, T> fn)
        {
            return new DelegateGenerator<T, TContextData>(fallbackId, fn);
        }

        throw new ArgumentException("Unsupported generator-like value", nameof(generatorLike));
    }
}

internal sealed class DelegateGenerator<T, TContextData>(
    string id,
    Func<Context<TContextData>, T> fn)
    : IGenerator<T, TContextData>
{
    public string Id { get; } = id;

    public T Generate(Context<TContextData> context) => fn(context);
}
