using LexiconLang.Core;

namespace LexiconLang.Grammar;

/// <summary>
/// Grammar rules dictionary. Values can be string, string[], Weighted&lt;string&gt;[],
/// IGenerator&lt;string, TContextData&gt;, or Func&lt;Context&lt;TContextData&gt;, string&gt;.
/// </summary>
public class GrammarRules<TContextData> : Dictionary<string, object>
{
    public GrammarRules() : base() { }
    public GrammarRules(IDictionary<string, object> dict) : base(dict) { }
}

public sealed record GrammarOptions<TContextData>(
    string? Id = null,
    string Start = "start",
    Dictionary<string, Modifier>? Modifiers = null,
    IRegistry? Registry = null,
    int MaxDepth = 64);

public class Grammar<TContextData> : IGenerator<string, TContextData>
{
    public string Id { get; }
    public IReadOnlyDictionary<string, object> Rules { get; }

    private readonly Dictionary<string, Modifier> _modifiers;
    private readonly IRegistry? _registry;
    private readonly int _maxDepth;
    private readonly string _startKey;
    private readonly Dictionary<string, CompiledRule> _compiled;

    public Grammar(GrammarRules<TContextData> rules, GrammarOptions<TContextData>? options = null)
    {
        var opts = options ?? new GrammarOptions<TContextData>();
        Id = opts.Id ?? "grammar";
        _startKey = opts.Start;
        _modifiers = new Dictionary<string, Modifier>(BuiltinModifiers.All);
        if (opts.Modifiers != null)
        {
            foreach (var (k, v) in opts.Modifiers)
                _modifiers[k] = v;
        }
        _registry = opts.Registry;
        _maxDepth = opts.MaxDepth;
        Rules = rules;
        _compiled = new Dictionary<string, CompiledRule>();

        foreach (var (key, value) in rules)
        {
            _compiled[key] = CompileRuleValue(value);
        }
    }

    public string Generate(Context<TContextData> context)
    {
        return Expand($"#{_startKey}#", context);
    }

    public string Expand(string template, Context<TContextData> ctx)
    {
        var evalCtx = new EvalContext(
            _modifiers,
            _compiled,
            _registry ?? ctx.Registry,
            _maxDepth);

        EvalContext.Push(evalCtx);
        try
        {
            var ast = Parser.Parse(template);
            return EvalNodes(ast, ctx, new LocalScope(null, new Dictionary<string, IReadOnlyList<SyntaxNode>>()), evalCtx);
        }
        finally
        {
            EvalContext.Pop();
        }
    }

    // ─── compiled rule ──────────────────────────────────────────────

    private delegate string CompiledRule(Context<TContextData> ctx, LocalScope locals);

    private CompiledRule CompileRuleValue(object value)
    {
        switch (value)
        {
            case string str:
                var ast = Parser.Parse(str);
                return (ctx, locals) => EvalNodes(ast, ctx, locals, EvalContext.Current!);

            case IReadOnlyList<string> arr:
                var asts = arr.Select(Parser.Parse).ToArray();
                return (ctx, locals) =>
                {
                    var idx = ctx.Rng.NextInt(0, arr.Count);
                    return EvalNodes(asts[idx], ctx, locals, EvalContext.Current!);
                };

            case IReadOnlyList<Weighted<string>> weighted:
                var wArr = weighted.ToArray();
                var wAsts = wArr.Select(w => Parser.Parse(w.Value)).ToArray();
                var table = Sample.BuildAliasTable(wArr);
                return (ctx, locals) =>
                {
                    var picked = Sample.SampleAlias(table, ctx.Rng);
                    var idx = Array.FindIndex(wArr, w => w.Value == picked);
                    if (idx < 0) idx = 0;
                    return EvalNodes(wAsts[idx], ctx, locals, EvalContext.Current!);
                };

            case Dictionary<string, double> dict:
                var nWeighted = Sample.NormalizeWeights(dict).ToArray();
                var nAsts = nWeighted.Select(w => Parser.Parse(w.Value)).ToArray();
                var nTable = Sample.BuildAliasTable(nWeighted);
                return (ctx, locals) =>
                {
                    var picked = Sample.SampleAlias(nTable, ctx.Rng);
                    var idx = Array.FindIndex(nWeighted, w => w.Value == picked);
                    if (idx < 0) idx = 0;
                    return EvalNodes(nAsts[idx], ctx, locals, EvalContext.Current!);
                };

            case IGenerator<string, TContextData> gen:
                return (ctx, _) => gen.Generate(ctx);

            case Func<Context<TContextData>, string> fn:
                return (ctx, _) => fn(ctx);

            default:
                return (_, _) => "";
        }
    }

    // ─── AST evaluation ─────────────────────────────────────────────

    private static string EvalNodes(
        IReadOnlyList<SyntaxNode> nodes,
        Context<TContextData> ctx,
        LocalScope locals,
        EvalContext ec)
    {
        if (ec.Depth > ec.MaxDepth)
            throw new InvalidOperationException($"Grammar: recursion depth exceeded ({ec.MaxDepth})");

        var output = "";
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];

            if (node is SyntaxNode.Text textNode)
            {
                output += textNode.Value;
            }
            else if (node is SyntaxNode.Raw rawNode)
            {
                output += rawNode.Value;
            }
            else if (node is SyntaxNode.Ref refNode)
            {
                // Process actions first: push variables onto local scope
                foreach (var action in refNode.Actions)
                {
                    locals.Vars[action.VariableName] = action.Rule;
                }

                if (refNode.Symbol == "__action__")
                    continue;

                var slotCtx = ctx.Child($"g:{i}:{refNode.Symbol}");
                var value = ExpandSymbol(refNode.Symbol, slotCtx, locals, ec);

                // Apply modifiers
                foreach (var mod in refNode.Modifiers)
                {
                    value = ApplyModifier(value, mod, ec);
                }

                output += value;
            }
        }

        return output;
    }

    private static string ExpandSymbol(
        string symbol,
        Context<TContextData> ctx,
        LocalScope locals,
        EvalContext ec)
    {
        // Check local scope variables
        var scope = locals;
        while (scope != null)
        {
            if (scope.Vars.TryGetValue(symbol, out var ruleNodes))
            {
                var nextScope = new LocalScope(scope, new Dictionary<string, IReadOnlyList<SyntaxNode>>());
                ec.Depth++;
                try
                {
                    return EvalNodes(ruleNodes, ctx, nextScope, ec);
                }
                finally
                {
                    ec.Depth--;
                }
            }
            scope = scope.Parent;
        }

        // Plugin lookup via colon prefix
        if (symbol.Contains(':') && ec.Registry?.Has(symbol) == true)
        {
            var gen = ec.Registry.Get<string, TContextData>(symbol);
            return gen.Generate(ctx);
        }

        // Look up compiled rule
        if (ec.Rules.TryGetValue(symbol, out var rule))
        {
            ec.Depth++;
            try
            {
                return rule(ctx, new LocalScope(locals, new Dictionary<string, IReadOnlyList<SyntaxNode>>()));
            }
            finally
            {
                ec.Depth--;
            }
        }

        // Fall back to registry lookup by bare name
        if (ec.Registry?.Has(symbol) == true)
        {
            return ec.Registry.Get<string, TContextData>(symbol).Generate(ctx);
        }

        // Not found — emit placeholder
        return $"(({symbol}))";
    }

    private static string ApplyModifier(string input, ModifierCall call, EvalContext ec)
    {
        if (!ec.Modifiers.TryGetValue(call.Name, out var fn))
            return input;
        return fn(input, call.Args);
    }

    // ─── nested types ───────────────────────────────────────────────

    private sealed class LocalScope
    {
        public LocalScope? Parent { get; }
        public Dictionary<string, IReadOnlyList<SyntaxNode>> Vars { get; }

        public LocalScope(LocalScope? parent, Dictionary<string, IReadOnlyList<SyntaxNode>> vars)
        {
            Parent = parent;
            Vars = vars;
        }
    }

    private sealed class EvalContext
    {
        [ThreadStatic] private static EvalContext? _current;

        public Dictionary<string, Modifier> Modifiers { get; }
        public Dictionary<string, CompiledRule> Rules { get; }
        public IRegistry? Registry { get; }
        public int MaxDepth { get; }
        public int Depth;

        public EvalContext(
            Dictionary<string, Modifier> modifiers,
            Dictionary<string, CompiledRule> rules,
            IRegistry? registry,
            int maxDepth)
        {
            Modifiers = modifiers;
            Rules = rules;
            Registry = registry;
            MaxDepth = maxDepth;
            Depth = 0;
        }

        public static EvalContext? Current => _current;
        public static void Push(EvalContext ctx) => _current = ctx;
        public static void Pop() => _current = null;
    }
}

public static class GrammarFactory
{
    public static Grammar<TContextData> Create<TContextData>(
        GrammarRules<TContextData> rules,
        GrammarOptions<TContextData>? options = null)
    {
        return new Grammar<TContextData>(rules, options);
    }
}
