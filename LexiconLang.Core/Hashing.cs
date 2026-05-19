namespace LexiconLang.Core;

public static class Hashing
{
    private const ulong FnvOffset64 = 0xcbf29ce484222325UL;
    private const ulong FnvPrime64 = 0x100000001b3UL;

    public static ulong Fnv1A64(string input)
    {
        var hash = FnvOffset64;
        foreach (var c in input)
        {
            hash ^= (byte)(c & 0xff);
            hash *= FnvPrime64;
            if (c > 0xff)
            {
                hash ^= (byte)((c >> 8) & 0xff);
                hash *= FnvPrime64;
            }
        }

        return hash;
    }

    public static ulong SplitMix64(ulong seed)
    {
        var z = seed + 0x9e3779b97f4a7c15UL;
        z = (z ^ (z >> 30)) * 0xbf58476d1ce4e5b9UL;
        z = (z ^ (z >> 27)) * 0x94d049bb133111ebUL;
        return z ^ (z >> 31);
    }

    public static ulong Mix(ulong parent, string label)
    {
        return SplitMix64(parent ^ Fnv1A64(label));
    }

    public static ulong SeedToUInt64(string seed)
    {
        return Fnv1A64(seed);
    }

    public static ulong SeedToUInt64(long seed)
    {
        return SplitMix64((ulong)seed);
    }
}
