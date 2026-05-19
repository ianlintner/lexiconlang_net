using LexiconLang.Cli;

var args = ArgParser.Parse(Environment.GetCommandLineArgs()[1..]);

switch (args.Command)
{
    case "build-markov":
        await Commands.BuildMarkovAsync(args);
        break;
    case "scaffold-pack":
        Commands.ScaffoldPack(args);
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
        Console.Error.WriteLine($"Unknown command: {args.Command}");
        Environment.Exit(1);
        break;
}
