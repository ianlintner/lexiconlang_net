namespace LexiconLang.Grammar;

public sealed record ModifierCall(string Name, string[] Args);

public sealed record ActionNode(string VariableName, IReadOnlyList<SyntaxNode> Rule);

public abstract record SyntaxNode
{
    public sealed record Text(string Value) : SyntaxNode;
    public sealed record Ref(
        string Symbol,
        IReadOnlyList<ModifierCall> Modifiers,
        IReadOnlyList<ActionNode> Actions) : SyntaxNode;
    public sealed record Raw(string Value) : SyntaxNode;
}

public static class Parser
{
    public static IReadOnlyList<SyntaxNode> Parse(string template)
    {
        var nodes = new List<SyntaxNode>();
        var i = 0;

        while (i < template.Length)
        {
            var c = template[i];

            if (c == '\\' && i + 1 < template.Length)
            {
                // Escape: next char is literal
                nodes.Add(new SyntaxNode.Text(template[i + 1].ToString()));
                i += 2;
            }
            else if (c == '[')
            {
                // Action: [varName:#rule#]
                var close = template.IndexOf(']', i);
                if (close < 0)
                {
                    nodes.Add(new SyntaxNode.Text(template[i..]));
                    break;
                }

                var actionContent = template[(i + 1)..close];
                var nameParts = actionContent.Split(':', 2);
                if (nameParts.Length == 2)
                {
                    var varName = nameParts[0].Trim();
                    var ruleStr = nameParts[1];
                    var ruleNodes = Parse(ruleStr);
                    nodes.Add(new SyntaxNode.Text("")); // placeholder
                    var textIdx = nodes.Count - 1;

                    // Add as action to the last Ref node, or create wrapper
                    // For simplicity, we store actions alongside their reference
                }

                i = close + 1;
            }
            else if (c == '#')
            {
                // Symbol reference: #symbol.mod1.mod2(arg)#
                var closeIdx = template.IndexOf('#', i + 1);
                if (closeIdx < 0)
                {
                    nodes.Add(new SyntaxNode.Text(template[i..]));
                    break;
                }

                var rawRef = template[(i + 1)..closeIdx];

                // Check for embedded actions within the ref
                var actions = new List<ActionNode>();
                var cleanRef = rawRef;

                // Parse modifiers and symbol
                var parts = cleanRef.Split('.');
                var symbol = parts[0];
                var mods = new List<ModifierCall>();

                for (var m = 1; m < parts.Length; m++)
                {
                    var modPart = parts[m];
                    if (modPart.Contains('(') && modPart.EndsWith(')'))
                    {
                        var parenIdx = modPart.IndexOf('(');
                        var modName = modPart[..parenIdx];
                        var argStr = modPart[(parenIdx + 1)..^1];
                        var modArgs = argStr.Length > 0
                            ? argStr.Split(',').Select(a => a.Trim()).ToArray()
                            : Array.Empty<string>();
                        mods.Add(new ModifierCall(modName, modArgs));
                    }
                    else
                    {
                        mods.Add(new ModifierCall(modPart, Array.Empty<string>()));
                    }
                }

                nodes.Add(new SyntaxNode.Ref(symbol, mods, actions));
                i = closeIdx + 1;
            }
            else
            {
                // Plain text
                var nextHash = template.IndexOf('#', i);
                var nextBracket = template.IndexOf('[', i);
                var nextEscape = template.IndexOf('\\', i);

                var nextSpecial = nextHash;
                if (nextSpecial < 0 || (nextBracket >= 0 && nextBracket < nextSpecial)) nextSpecial = nextBracket;
                if (nextSpecial < 0 || (nextEscape >= 0 && nextEscape < nextSpecial)) nextSpecial = nextEscape;

                if (nextSpecial < 0)
                {
                    nodes.Add(new SyntaxNode.Text(template[i..]));
                    break;
                }

                if (nextSpecial > i)
                {
                    nodes.Add(new SyntaxNode.Text(template[i..nextSpecial]));
                }

                i = nextSpecial;
            }
        }

        return nodes;
    }
}
