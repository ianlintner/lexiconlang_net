using LexiconLang.Cli;

var parsed = ArgParser.Parse(Environment.GetCommandLineArgs()[1..]);

switch (parsed.Command)
{
    case "build-markov":
        await Commands.BuildMarkovAsync(parsed);
        break;
    case "scaffold-pack":
        Commands.ScaffoldPack(parsed);
        break;
    case "help":
    case "--help":
    case "-h":
    case null:
        Console.WriteLine(@"LexiconLang CLI
  build-markov <input.json> --out <model.json> [--order 3] [--min-length 3] [--max-length 14]
  scaffold-pack <name> [--dir ./packages]
  help");
        break;
    default:
        Console.Error.WriteLine($"Unknown command: {parsed.Command}");
        Environment.Exit(1);
        break;
}
