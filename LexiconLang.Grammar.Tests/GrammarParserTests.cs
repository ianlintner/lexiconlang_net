using LexiconLang.Core;
using LexiconLang.Grammar;

namespace LexiconLang.Grammar.Tests;

public class GrammarParserTests
{
    [Fact]
    public void SimpleRuleExpands()
    {
        var rules = new Dictionary<string, object>
        {
            ["greeting"] = "hello"
        };

        var grammar = GrammarFactory.Create(new GrammarRules<object?>(rules),
            new GrammarOptions<object?>("test", "greeting"));

        var ctx = LexiconContext.Create("parse");
        var result = grammar.Generate(ctx);

        Assert.Equal("hello", result);
    }

    [Fact]
    public void ArrayRulePicksElement()
    {
        var rules = new Dictionary<string, object>
        {
            ["greeting"] = new[] { "hello", "hi", "hey" }
        };

        var grammar = GrammarFactory.Create(new GrammarRules<object?>(rules),
            new GrammarOptions<object?>("test", "greeting"));

        var ctx = LexiconContext.Create("test");
        var result = grammar.Generate(ctx);

        Assert.Contains(result, new[] { "hello", "hi", "hey" });
    }

    [Fact]
    public void ReferencedRuleExpands()
    {
        var rules = new Dictionary<string, object>
        {
            ["greeting"] = "hello #name#",
            ["name"] = new[] { "Alice", "Bob" }
        };

        var grammar = GrammarFactory.Create(new GrammarRules<object?>(rules),
            new GrammarOptions<object?>("test", "greeting"));

        var ctx = LexiconContext.Create("ref");
        var result = grammar.Generate(ctx);

        Assert.StartsWith("hello ", result);
        Assert.Contains(result, new[] { "hello Alice", "hello Bob" });
    }

    [Fact]
    public void PipeModifierWorksWithCapitalize()
    {
        var modifiers = new Dictionary<string, Modifier>
        {
            ["capitalize"] = BuiltinModifiers.CapitalizeMod
        };

        var rules = new Dictionary<string, object>
        {
            ["word"] = "hello"
        };

        var grammar = GrammarFactory.Create(new GrammarRules<object?>(rules),
            new GrammarOptions<object?>("test", "word", modifiers));

        var ctx = LexiconContext.Create("mod");
        var result = grammar.Expand("#word|capitalize#", ctx);

        Assert.Equal("Hello", result);
    }

    [Fact]
    public void MultipleModifiersChain()
    {
        var modifiers = new Dictionary<string, Modifier>
        {
            ["upper"] = BuiltinModifiers.UpperMod,
            ["trim"] = BuiltinModifiers.TrimMod
        };

        var rules = new Dictionary<string, object>
        {
            ["word"] = "  hello  "
        };

        var grammar = GrammarFactory.Create(new GrammarRules<object?>(rules),
            new GrammarOptions<object?>("test", "word", modifiers));

        var ctx = LexiconContext.Create("chain");
        var result = grammar.Expand("#word|trim|upper#", ctx);

        Assert.Equal("HELLO", result);
    }

    [Fact]
    public void RecursionIsLimited()
    {
        var rules = new Dictionary<string, object>
        {
            ["recursive"] = "#recursive#"
        };

        var grammar = GrammarFactory.Create(new GrammarRules<object?>(rules),
            new GrammarOptions<object?>("test", "recursive", MaxDepth: 5));

        var ctx = LexiconContext.Create("recurse");

        // Grammar throws InvalidOperationException when recursion depth is exceeded
        Assert.Throws<InvalidOperationException>(() => grammar.Generate(ctx));
    }

    [Fact]
    public void LiteralTextPreserved()
    {
        var rules = new Dictionary<string, object>
        {
            ["sentence"] = "The quick brown fox jumps over the lazy dog"
        };

        var grammar = GrammarFactory.Create(new GrammarRules<object?>(rules),
            new GrammarOptions<object?>("test", "sentence"));

        var ctx = LexiconContext.Create("literal");
        var result = grammar.Generate(ctx);

        Assert.Equal("The quick brown fox jumps over the lazy dog", result);
    }

    [Fact]
    public void WeightedRulesHonorWeights()
    {
        var rules = new Dictionary<string, object>
        {
            ["die"] = new Dictionary<string, double>
            {
                ["1"] = 1,
                ["2"] = 1,
                ["3"] = 1,
                ["4"] = 1,
                ["5"] = 1,
                ["6"] = 1
            }
        };

        var grammar = GrammarFactory.Create(new GrammarRules<object?>(rules),
            new GrammarOptions<object?>("test", "die"));

        var ctx = LexiconContext.Create("weighted");
        var results = new HashSet<string>();

        for (var i = 0; i < 100; i++)
        {
            var result = grammar.Generate(ctx.Child($"roll:{i}"));
            results.Add(result);
        }

        // With 100 rolls of a d6, we should see most outcomes
        Assert.True(results.Count >= 4);
    }
}
