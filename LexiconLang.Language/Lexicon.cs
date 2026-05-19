using LexiconLang.Core;

namespace LexiconLang.Language;

public class Lexicon
{
    private readonly Dictionary<string, string> _cache = new();
    private readonly Culture _culture;
    private readonly Context<object?> _cultureCtx;
    private readonly Dictionary<string, Meaning> _allMeanings;

    public string CultureId => _culture.Id;

    internal Lexicon(Culture culture, Context<object?> cultureCtx, Dictionary<string, Meaning> allMeanings)
    {
        _culture = culture;
        _cultureCtx = cultureCtx;
        _allMeanings = allMeanings;
    }

    public string FormOf(string meaningId)
    {
        if (_cache.TryGetValue(meaningId, out var cached))
            return cached;

        // Key-addressed fork: order-independent and patch-stable
        var wordCtx = _cultureCtx.Child($"word:{meaningId}");
        var form = Phonotactics.GenerateWord(_culture.Glyphs, wordCtx);
        _cache[meaningId] = form;
        return form;
    }

    public Meaning[] ByClass(WordClass wordClass, string? tag = null)
    {
        return _culture.MeaningPacks
            .SelectMany(p => p.Meanings)
            .Where(m => m.Class == wordClass && (tag == null || m.Tags.Contains(tag)))
            .ToArray();
    }

    public IReadOnlyDictionary<string, string> Materialize()
    {
        var result = new Dictionary<string, string>();
        foreach (var meaningId in _allMeanings.Keys)
        {
            result[meaningId] = FormOf(meaningId);
        }
        return result;
    }
}

public static class LexiconBuilder
{
    public static Lexicon BuildLexicon<TContextData>(Culture culture, Context<TContextData> ctx)
    {
        var cultureCtx = ctx.Child($"lang:{culture.Id}").WithData<object?>(null!);
        var allMeanings = new Dictionary<string, Meaning>();
        foreach (var pack in culture.MeaningPacks)
        {
            foreach (var meaning in pack.Meanings)
            {
                allMeanings[meaning.Id] = meaning;
            }
        }
        return new Lexicon(culture, cultureCtx, allMeanings);
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lexiconlang_net
{
    public class Lexicon
    {
        
    }
}