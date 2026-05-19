using LexiconLang.Markov;
using System.Text.Json;

namespace LexiconLang.Cli;

public static class Commands
{
    public static async Task BuildMarkovAsync(ParsedArgs args)
    {
        var inputFile = args.Positional.FirstOrDefault()
            ?? throw new InvalidOperationException("Usage: build-markov <input.json> --out <model.json>");
        var outputFile = ArgParser.RequireOption(args, "out");
        var order = ArgParser.IntOption(args, "order", 3);
        var minLength = ArgParser.IntOption(args, "min-length", 3);
        var maxLength = ArgParser.IntOption(args, "max-length", 14);
        var rejectLen = args.Options.ContainsKey("reject-substrings-of-length")
            ? ArgParser.IntOption(args, "reject-substrings-of-length", 0)
            : (int?)null;
        var lowercase = !ArgParser.BoolOption(args, "no-lowercase");

        var json = await File.ReadAllTextAsync(inputFile);
        var corpus = JsonSerializer.Deserialize<List<TrainEntry>>(json)
                     ?? throw new InvalidOperationException("Invalid input JSON");

        var model = Trainer.Train(corpus, new TrainOptions(
            Order: order,
            MinLength: minLength,
            MaxLength: maxLength,
            RejectSubstringsOfLength: rejectLen > 0 ? rejectLen : null,
            Lowercase: lowercase));

        var outputJson = model.ToJson();
        await File.WriteAllTextAsync(outputFile, outputJson);
        Console.WriteLine($"Wrote Markov model to {outputFile} (order={model.Order}, {model.Transitions.Count} contexts)");
    }

    public static void ScaffoldPack(ParsedArgs args)
    {
        var name = args.Positional.FirstOrDefault()
            ?? throw new InvalidOperationException("Usage: scaffold-pack <name> [--dir <path>]");
        var dir = args.Options.TryGetValue("dir", out var d) && d is string s ? s : "./packages";
        var packDir = Path.Combine(dir, name);

        Directory.CreateDirectory(packDir);

        File.WriteAllText(Path.Combine(packDir, $"{name}.csproj"), $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include=""..\..\LexiconLang.Core\LexiconLang.Core.csproj"" />
  </ItemGroup>
</Project>");

        Directory.CreateDirectory(Path.Combine(packDir, "src"));
        File.WriteAllText(Path.Combine(packDir, "src", "Index.cs"), $@"namespace LexiconLang.{name};

public static class {name}
{{
    // Your generators go here
}}");

        Console.WriteLine($"Scaffolded pack '{name}' in {packDir}");
    }
}
