using LexiconLang.Core;

namespace LexiconLang.Modern.Tests;

public class ModernNamesTests
{
    [Fact]
    public void ModernNamesArraysArePopulated()
    {
        Assert.NotEmpty(ModernNames.GivenMale);
        Assert.NotEmpty(ModernNames.GivenFemale);
        Assert.NotEmpty(ModernNames.Surnames);
    }

    [Fact]
    public void GenerateProducesNonEmptyName()
    {
        var ctx = LexiconContext.Create("modern-test");
        var name = ModernNames.Generate(ctx);

        Assert.False(string.IsNullOrEmpty(name.Given));
        Assert.False(string.IsNullOrEmpty(name.Surname));
        Assert.False(string.IsNullOrEmpty(name.Full));
    }

    [Fact]
    public void GenerateIsDeterministic()
    {
        var ctx1 = LexiconContext.Create("modern-seed");
        var name1 = ModernNames.Generate(ctx1);

        var ctx2 = LexiconContext.Create("modern-seed");
        var name2 = ModernNames.Generate(ctx2);

        Assert.Equal(name1.Full, name2.Full);
        Assert.Equal(name1.Sex, name2.Sex);
    }

    [Fact]
    public void FullNameCombinesGivenAndSurname()
    {
        var ctx = LexiconContext.Create("name-full");
        var name = ModernNames.Generate(ctx);

        Assert.Equal($"{name.Given} {name.Surname}", name.Full);
    }

    [Fact]
    public void DifferentSeedsDifferentNames()
    {
        var name1 = ModernNames.Generate(LexiconContext.Create("seed-a"));
        var name2 = ModernNames.Generate(LexiconContext.Create("seed-b"));

        // Different seeds should yield different results (overwhelmingly likely)
        Assert.False(name1.Full == name2.Full && name1.Sex == name2.Sex);
    }

    [Fact]
    public void GenerateCityNameProducesNonEmptyString()
    {
        var ctx = LexiconContext.Create("city-test");
        var city = ModernNames.CityName.Generate(ctx);
        Assert.False(string.IsNullOrEmpty(city));
    }

    [Fact]
    public void GenerateStreetNameProducesNonEmptyString()
    {
        var ctx = LexiconContext.Create("street-test");
        var street = ModernNames.StreetName.Generate(ctx);
        Assert.False(string.IsNullOrEmpty(street));
    }

    [Fact]
    public void GenerateCompanyNameProducesNonEmptyString()
    {
        var ctx = LexiconContext.Create("company-test");
        var company = ModernNames.CompanyName.Generate(ctx);
        Assert.False(string.IsNullOrEmpty(company));
    }
}
