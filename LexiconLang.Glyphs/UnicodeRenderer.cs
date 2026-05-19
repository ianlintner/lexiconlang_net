namespace LexiconLang.Glyphs;

public sealed record UnicodeConfig(
    Dictionary<string, string>? Mappings = null,
    string Fallback = "⬡");

public static class UnicodeRenderer
{
    public static readonly Dictionary<string, string> Registry = new()
    {
        ["strong"] = "💪",
        ["anvil"] = "⚒",
        ["gem"] = "💎",
        ["mountain"] = "⛰",
        ["fire"] = "🔥",
        ["water"] = "💧",
        ["tree"] = "🌳",
        ["sky"] = "☁️",
        ["star"] = "⭐",
        ["moon"] = "🌙",
        ["sun"] = "☀️",
        ["storm"] = "⛈",
        ["snow"] = "❄",
        ["wolf"] = "🐺",
        ["eagle"] = "🦅",
        ["bear"] = "🐻",
        ["dragon"] = "🐉",
        ["lion"] = "🦁",
        ["horse"] = "🐴",
        ["fish"] = "🐟",
        ["spider"] = "🕷",
        ["crow"] = "🐦",
        ["owl"] = "🦉",
        ["sword"] = "⚔",
        ["crown"] = "👑",
        ["shield"] = "🛡",
        ["heart"] = "❤️",
        ["eye"] = "👁",
        ["bone"] = "🦴",
        ["blood"] = "🩸",
        ["flame"] = "🔥",
        ["wave"] = "🌊",
        ["light"] = "💡",
        ["shadow"] = "🌑",
        ["death"] = "💀",
        ["life"] = "🌱",
        ["music"] = "🎵",
        ["song"] = "🎶",
        ["love"] = "💕",
        ["hate"] = "💔",
        ["peace"] = "☮",
        ["war"] = "⚔",
        ["swift"] = "💨",
        ["brave"] = "🦁",
        ["wise"] = "🦉",
    };

    public static string RenderToUnicode(string meaning, UnicodeConfig? config = null)
    {
        var mappings = config?.Mappings ?? Registry;
        var fallback = config?.Fallback ?? "⬡";

        var key = meaning.ToLowerInvariant().Trim();
        return mappings.GetValueOrDefault(key, fallback);
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lexiconlang_net
{
    public class UnicodeRenderer
    {
        
    }
}