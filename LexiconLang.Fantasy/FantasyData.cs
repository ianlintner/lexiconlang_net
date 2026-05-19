namespace LexiconLang.Fantasy;

public static class FantasyData
{
    public static readonly Dictionary<string, string[]> Adjectives = new()
    {
        ["noble"] = new[] { "Valiant", "Righteous", "Honored", "Stalwart", "True", "Brave", "Dauntless", "Resolute" },
        ["sinister"] = new[] { "Cruel", "Bloody", "Vile", "Wicked", "Dread", "Cursed", "Fell", "Black" },
        ["rustic"] = new[] { "Drunken", "Salty", "Crooked", "Lazy", "Old", "Sleeping", "Roaring", "Whistling" },
        ["shiny"] = new[] { "Gilded", "Silver", "Golden", "Brass", "Iron", "Copper", "Glowing", "Shining" },
        ["natural"] = new[] { "Wild", "Stormy", "Misty", "Sunlit", "Frozen", "Burning", "Verdant", "Rolling" },
        ["mystical"] = new[] { "Arcane", "Whispering", "Forgotten", "Eldritch", "Hidden", "Ancient", "Veiled", "Echoing" },
    };

    public static readonly string[] Animals =
    {
        "Wolf", "Bear", "Stag", "Hawk", "Lion", "Raven", "Boar", "Falcon",
        "Owl", "Fox", "Serpent", "Dragon", "Wyvern", "Griffin", "Unicorn",
        "Phoenix", "Tiger", "Eagle", "Crow", "Spider", "Hound",
    };

    public static readonly string[] TavernNouns =
    {
        "Anchor", "Dragon", "Coin", "Goblet", "Tankard", "Crown", "Boot",
        "Fiddle", "Lantern", "Pony", "Mast", "Hammer", "Goose", "Pig",
        "Mermaid", "Shield", "Torch", "Compass", "Wheel",
    };

    public static readonly string[] PlacePrefixes =
    {
        "Black", "White", "Red", "Silver", "Gold", "Iron", "Stone", "Bone",
        "Star", "Moon", "Sun", "Wind", "Storm", "Frost", "Fire", "Shadow",
        "Mist", "Thorn", "Wolf", "Raven", "Oak", "Ash", "Pine",
    };

    public static readonly Dictionary<string, string[]> PlaceSuffixes = new()
    {
        ["settlement"] = new[] { "stead", "burgh", "ford", "hold", "haven", "wick", "by", "ton", "vale", "march", "reach", "hollow", "mire", "fall" },
        ["hill"] = new[] { "mount", "tor", "peak", "crag", "spire", "horn", "fang" },
        ["water"] = new[] { "mere", "tarn", "pool", "brook", "river", "lake", "sound" },
        ["forest"] = new[] { "wood", "wald", "thicket", "grove", "wilds" },
    };

    public static readonly string[] FactionTypes =
    {
        "Order", "Brotherhood", "Sisterhood", "Cabal", "League", "Circle",
        "Conclave", "Company", "Guild", "Covenant", "Pact", "Hand",
    };

    public static readonly string[] Occupations =
    {
        "blacksmith", "innkeeper", "merchant", "farmer", "soldier", "guard",
        "priest", "scholar", "thief", "ranger", "minstrel", "mercenary",
        "alchemist", "scribe", "hunter", "fisher", "miller", "tanner",
        "butcher", "baker", "weaver", "stonecutter",
    };

    public static readonly Dictionary<string, string[]> PersonalityTraits = new()
    {
        ["positive"] = new[] { "loyal", "honest", "brave", "kind", "patient", "wise", "humble" },
        ["negative"] = new[] { "cruel", "greedy", "cowardly", "vain", "lazy", "stubborn", "boastful" },
        ["quirks"] = new[]
        {
            "speaks only in proverbs", "collects buttons", "fears birds",
            "always whistles", "never removes their hat", "carries a lucky coin",
            "knows seven languages but speaks only in one",
        },
    };

    public static readonly string[] Weapons =
        { "Blade", "Sword", "Axe", "Hammer", "Bow", "Spear", "Dagger", "Staff", "Mace", "Halberd", "Glaive" };

    public static readonly string[] ArmorPieces =
        { "Shield", "Helm", "Gauntlet", "Cuirass", "Greaves", "Mantle" };

    public static readonly string[] EpithetActions =
        { "Slayer", "Bane", "Breaker", "Bringer", "Keeper", "Singer", "Walker", "Hunter", "Whisperer", "Caller", "Eater", "Forger" };

    public static readonly string[] EpithetTargets =
        { "Dragon", "Wolf", "Storm", "Shadow", "Star", "Bone", "Iron", "Doom", "Fate", "Crown", "Ash", "Frost", "Flame" };

    public static readonly Dictionary<string, string[]> Titles = new()
    {
        ["noble"] = new[] { "Lord", "Lady", "Duke", "Duchess", "Count", "Countess", "Baron", "Baroness" },
        ["martial"] = new[] { "Captain", "Commander", "Marshal", "General", "Warlord", "Champion" },
        ["arcane"] = new[] { "Archmage", "Sorcerer", "Enchanter", "Warlock", "Seer", "Oracle" },
        ["divine"] = new[] { "High Priest", "Templar", "Paladin", "Cleric", "Inquisitor" },
    };

    public static readonly string[] DragonAdjectives =
        { "Ancient", "Elder", "Great", "Dread", "Shadow", "Storm", "Fire", "Frost", "Venom", "Star" };

    public static readonly string[] DragonColors =
        { "Red", "Blue", "Green", "Black", "White", "Gold", "Silver", "Bronze", "Copper", "Brass" };

    public static readonly string[] QuestHookOpenings =
        { "A mysterious", "An ancient", "A forgotten", "The last", "A fallen", "A cursed", "The hidden" };

    public static readonly string[] QuestHookSubjects =
        { "artifact", "relic", "tome", "prophecy", "crown", "blade", "temple", "tomb" };

    public static readonly string[] QuestHookComplications =
        {
            "has been stolen", "lies in ruins", "is guarded by a dragon",
            "holds a dark secret", "was never meant to be found",
            "awakens an ancient evil",
        };
}
