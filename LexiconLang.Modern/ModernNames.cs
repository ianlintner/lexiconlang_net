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

    public static readonly IGenerator<string, object?> GivenMaleName =
        Combinators.OneOf<string, object?>(GivenMale);

    public static readonly IGenerator<string, object?> GivenFemaleName =
        Combinators.OneOf<string, object?>(GivenFemale);

    public static readonly IGenerator<string, object?> Surname =
        Combinators.OneOf<string, object?>(Surnames);

    public static readonly IGenerator<Sex, object?> SexGenerator =
        Combinators.OneOf<Sex, object?>(Sex.Male, Sex.Female);

    public static PersonName Generate(Context<object?> ctx)
    {
        var sex = SexGenerator.Generate(ctx.Child("sex"));
        var given = (sex == Sex.Male ? GivenMaleName : GivenFemaleName).Generate(ctx.Child("given"));
        var sur = Surname.Generate(ctx.Child("surname"));
        return new PersonName(given, sur, sex);
    }
}
