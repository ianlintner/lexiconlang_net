namespace LexiconLang.Core;

public readonly record struct RngState(uint A, uint B, uint C, uint D, string Origin);

public interface IRng
{
    double Next();
    uint NextUInt32();
    int NextInt(int min, int maxExclusive);
    double NextRange(double min, double max);
    T Pick<T>(IReadOnlyList<T> items);
    RngState State();
    IRng Fork(string label);
    IRng Fork(int label);
}

public sealed class Sfc32 : IRng
{
    private const double U32 = 4294967296d;

    private uint _a;
    private uint _b;
    private uint _c;
    private uint _d;
    private readonly ulong _origin;
    private readonly string _originLabel;

    public Sfc32(string seed)
    {
        _origin = Hashing.SeedToUInt64(seed);
        _originLabel = seed;
        SeedFromUlong(_origin);
    }

    public Sfc32(long seed)
    {
        _origin = Hashing.SeedToUInt64(seed);
        _originLabel = _origin.ToString("x");
        SeedFromUlong(_origin);
    }

    public Sfc32(ulong seed, string? originLabel = null)
    {
        _origin = seed;
        _originLabel = originLabel ?? seed.ToString("x");
        SeedFromUlong(seed);
    }

    public Sfc32(RngState state)
    {
        _a = state.A;
        _b = state.B;
        _c = state.C;
        _d = state.D;
        _origin = Hashing.SeedToUInt64(state.Origin);
        _originLabel = state.Origin;
    }

    private void SeedFromUlong(ulong seed)
    {
        var lo = (uint)(seed & 0xffffffffUL);
        var hi = (uint)((seed >> 32) & 0xffffffffUL);

        _a = lo ^ 0x9e3779b9U;
        _b = hi ^ 0x243f6a88U;
        _c = lo + hi;
        _d = lo ^ hi ^ 0xb7e15162U;
        for (var i = 0; i < 12; i++)
        {
            NextUInt32();
        }
    }

    public uint NextUInt32()
    {
        var t = _a + _b + _d;
        _d += 1;
        _a = _b ^ (_b >> 9);
        _b = _c + (_c << 3);
        _c = (_c << 21) | (_c >> 11);
        _c += t;
        return t;
    }

    public double Next()
    {
        return NextUInt32() / U32;
    }

    public int NextInt(int min, int maxExclusive)
    {
        if (maxExclusive <= min) return min;
        var range = maxExclusive - min;
        return min + (int)Math.Floor(Next() * range);
    }

    public double NextRange(double min, double max)
    {
        return min + Next() * (max - min);
    }

    public T Pick<T>(IReadOnlyList<T> items)
    {
        if (items.Count == 0) throw new InvalidOperationException("Pick: empty list");
        return items[NextInt(0, items.Count)];
    }

    public RngState State()
    {
        return new RngState(_a, _b, _c, _d, _originLabel);
    }

    public IRng Fork(string label)
    {
        var childSeed = Hashing.Mix(_origin, label);
        return new Sfc32(childSeed, $"{_originLabel}/{label}");
    }

    public IRng Fork(int label)
    {
        return Fork($"i:{label}");
    }
}

public static class Rng
{
    public static IRng Create(string seed) => new Sfc32(seed);
    public static IRng Create(long seed) => new Sfc32(seed);
    public static IRng Create(ulong seed) => new Sfc32(seed);
    public static IRng Create(RngState state) => new Sfc32(state);
}
