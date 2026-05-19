using LexiconLang.Core;

namespace LexiconLang.Modern;

public enum Sex { Male, Female }

public sealed record PersonName(string Given, string Surname, Sex Sex)
{
    public string Full => $"{Given} {Surname}";
}

public static class ModernNames
{
    public static readonly string[] GivenMale =
    {
        "Aiden", "Alex", "Ben", "Carlos", "Daniel", "Ethan", "Felix", "Gabriel",
        "Hassan", "Isaac", "Jayden", "Kenji", "Liam", "Marcus", "Noah",
        "Omar", "Parker", "Quinn", "Ravi", "Samir", "Theo", "Umar",
        "Victor", "Wyatt", "Xavier", "Yusuf", "Zane", "Anders", "Bo",
        "Cole", "Diego", "Eli", "Finn", "George",
    };

    public static readonly string[] GivenFemale =
    {
        "Aaliyah", "Beatrice", "Camila", "Diana", "Elena", "Fatima", "Grace",
        "Hannah", "Imani", "Julia", "Kira", "Lina", "Mira", "Naomi",
        "Olivia", "Priya", "Quinn", "Riya", "Sofia", "Tara", "Una",
        "Valeria", "Wren", "Xiomara", "Yara", "Zara", "Anya", "Brielle",
        "Chloe", "Daria", "Eve",
    };

    public static readonly string[] Surnames =
    {
        "Adams", "Brooks", "Chen", "Davis", "Evans", "Fischer", "Garcia",
        "Hayashi", "Ibarra", "Johnson", "Kowalski", "Lopez", "Martinez",
        "Nguyen", "Okonkwo", "Patel", "Quintero", "Robinson", "Smith",
        "Tanaka", "Uddin", "Vargas", "Wilson", "Xu", "Yamamoto", "Zhao",
        "Anderson", "Brown", "Clark", "Diaz", "Edwards", "Foster",
    };

    public static readonly string[] CityPrefixes = { "North", "South", "East", "West", "New", "Port", "Fort", "Mount", "Lake", "Point" };
    public static readonly string[] CityProperNouns = { "Spring", "Brook", "Oak", "Pine", "Maple", "Cedar", "Ash", "Elm", "River", "Stone", "Hill", "Green" };
    public static readonly string[] CitySuffixes = { "ville", "ton", "burg", "bury", "wood", "field", "port", "land", "water", "city" };

    public static readonly string[] StreetOrdinals = { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th", "10th" };
    public static readonly string[] StreetTreeTypes = { "Oak", "Pine", "Maple", "Cedar", "Ash", "Elm", "Birch", "Spruce", "Walnut", "Willow" };
    public static readonly string[] StreetTypes = { "Street", "Avenue", "Boulevard", "Lane", "Drive", "Road", "Court", "Place", "Way", "Cove" };

    public static readonly string[] CompanyWords = { "Global", "Universal", "Acme", "Apex", "Zenith", "Summit", "Pioneer", "Vanguard", "Nexus", "Meridian" };
    public static readonly string[] CompanySuffixes = { "Inc.", "LLC", "Corp", "Solutions", "Systems", "Technologies", "Group", "Enterprises", "Partners" };

    public static readonly IGenerator<string, object?> GivenMaleName =
        Combinators.OneOf<string, object?>(GivenMale);

    public static readonly IGenerator<string, object?> GivenFemaleName =
        Combinators.OneOf<string, object?>(GivenFemale);

    public static readonly IGenerator<string, object?> Surname =
        Combinators.OneOf<string, object?>(Surnames);

    public static readonly IGenerator<Sex, object?> SexGenerator =
        Combinators.OneOf<Sex, object?>(Sex.Male, Sex.Female);

    public static readonly IGenerator<string, object?> CityName =
        Combinators.Compose<string, object?>("modern.cityName", ctx =>
        {
            var choice = ctx.Rng.Next();
            if (choice < 0.4) // Prefix + Proper
            {
                var pre = Combinators.OneOf<string, object?>(CityPrefixes).Generate(ctx.Child("pre"));
                var prop = Combinators.OneOf<string, object?>(CityProperNouns).Generate(ctx.Child("prop"));
                return $"{pre} {prop}";
            }
            else if (choice < 0.8) // Proper + Suffix
            {
                var prop = Combinators.OneOf<string, object?>(CityProperNouns).Generate(ctx.Child("prop"));
                var suf = Combinators.OneOf<string, object?>(CitySuffixes).Generate(ctx.Child("suf"));
                return $"{prop}{suf}";
            }
            else // Prefix + Proper + Suffix
            {
                var pre = Combinators.OneOf<string, object?>(CityPrefixes).Generate(ctx.Child("pre"));
                var prop = Combinators.OneOf<string, object?>(CityProperNouns).Generate(ctx.Child("prop"));
                var suf = Combinators.OneOf<string, object?>(CitySuffixes).Generate(ctx.Child("suf"));
                return $"{pre} {prop}{suf}";
            }
        });

    public static readonly IGenerator<string, object?> StreetName =
        Combinators.Compose<string, object?>("modern.streetName", ctx =>
        {
            var choice = ctx.Rng.Next();
            if (choice < 0.5) // Ordinal + Type
            {
                var ord = Combinators.OneOf<string, object?>(StreetOrdinals).Generate(ctx.Child("ord"));
                var type = Combinators.OneOf<string, object?>(StreetTypes).Generate(ctx.Child("type"));
                return $"{ord} {type}";
            }
            else if (choice < 0.8) // Tree + Type
            {
                var tree = Combinators.OneOf<string, object?>(StreetTreeTypes).Generate(ctx.Child("tree"));
                var type = Combinators.OneOf<string, object?>(StreetTypes).Generate(ctx.Child("type"));
                return $"{tree} {type}";
            }
            else // Surname + Type
            {
                var sur = Combinators.OneOf<string, object?>(Surnames).Generate(ctx.Child("sur"));
                var type = Combinators.OneOf<string, object?>(StreetTypes).Generate(ctx.Child("type"));
                return $"{sur} {type}";
            }
        });

    public static readonly IGenerator<string, object?> CompanyName =
        Combinators.Compose<string, object?>("modern.companyName", ctx =>
        {
            var choice = ctx.Rng.Next();
            if (choice < 0.5) // Word + Suffix
            {
                var word = Combinators.OneOf<string, object?>(CompanyWords).Generate(ctx.Child("word"));
                var suf = Combinators.OneOf<string, object?>(CompanySuffixes).Generate(ctx.Child("suf"));
                return $"{word} {suf}";
            }
            else // Surname + Suffix
            {
                var sur = Combinators.OneOf<string, object?>(Surnames).Generate(ctx.Child("sur"));
                var suf = Combinators.OneOf<string, object?>(CompanySuffixes).Generate(ctx.Child("suf"));
                return $"{sur} {suf}";
            }
        });

    public static PersonName Generate(Context<object?> ctx)
    {
        var sex = SexGenerator.Generate(ctx.Child("sex"));
        var given = (sex == Sex.Male ? GivenMaleName : GivenFemaleName).Generate(ctx.Child("given"));
        var sur = Surname.Generate(ctx.Child("surname"));
        return new PersonName(given, sur, sex);
    }
}
