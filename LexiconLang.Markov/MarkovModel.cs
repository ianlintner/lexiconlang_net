using System.Text.Json;
using System.Text.Json.Serialization;

namespace LexiconLang.Markov;

public static class MarkovTokens
{
    public const string Start = "^";
    public const string End = "$";
}

public sealed class MarkovModel
{
    public int Order { get; set; } = 3;
    public int MinLength { get; set; } = 3;
    public int MaxLength { get; set; } = 14;
    public Dictionary<string, Dictionary<string, int>> Transitions { get; set; } = new();
    public HashSet<string>? Forbidden { get; set; }
    public Dictionary<string, object?>? Meta { get; set; }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, MarkovJsonContext.Default.MarkovModel);
    }

    public static MarkovModel FromJson(string json)
    {
        return JsonSerializer.Deserialize(json, MarkovJsonContext.Default.MarkovModel)
               ?? throw new InvalidOperationException("Failed to deserialize MarkovModel");
    }
}

[JsonSerializable(typeof(MarkovModel))]
[JsonSerializable(typeof(Dictionary<string, Dictionary<string, int>>))]
[JsonSerializable(typeof(Dictionary<string, object?>))]
[JsonSerializable(typeof(HashSet<string>))]
internal partial class MarkovJsonContext : JsonSerializerContext
{
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lexiconlang_net
{
    public class MarkovModel
    {
        
    }
}