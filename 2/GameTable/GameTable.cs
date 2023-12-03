using System.Text.RegularExpressions;

namespace GameTable;


public class GameTable
{


  readonly static Regex GAME_LINE = new Regex(@"Game (?<id>\d+):(?<other>.*)");

  readonly static Func<string, IEnumerable<string>> readFile = File.ReadLines;

  public readonly static Func<string, Set> parseSet = _ =>
  {
    var cubes = _.Trim().Split(",");

    Dictionary<string, int> resultsAsMap = cubes
          .Select(_ => _.Trim())
          .Select(_ => _.Split(" "))
          .Where(_ => _.Length > 1)
          .ToDictionary(e => e[1], e => int.Parse(e[0]));

    return new Set
    {
      Red = resultsAsMap.GetValueOrDefault("red", 0),
      Green = resultsAsMap.GetValueOrDefault("green", 0),
      Blue = resultsAsMap.GetValueOrDefault("blue", 0),
    };
  };

  public static IEnumerable<Game> loadCalibrationsFromFile(string path)
  {
    return readFile.Invoke(path)
      .Select(ParseLineToGame);
  }

  public static Game ParseLineToGame(string line)
  {
    var match = GAME_LINE.Match(line);

    if (!match.Success) {
      Console.WriteLine($"no match for {line}");
      return null;
    }

    var id = int.Parse(match.Groups["id"].Value);

    var sets = match.Groups["other"].Value
        .Split(";")
        .Select(parseSet.Invoke);

    return new Game { Id = id, Sets = sets };
  }

  public static int SumOfValidGameIds(IEnumerable<Game> games, Configuration configuration)
  {
    Configuration configurationSafe = configuration ?? new Configuration { Red = int.MaxValue, Blue = int.MaxValue, Green = int.MaxValue };

    Predicate<Game> isGamePossible = game =>
    {
      bool isRedPossible = game.Sets.Select(_ => _.Red).Max() <= configurationSafe.Red;
      bool isGreenPossible = game.Sets.Select(_ => _.Green).Max() <= configurationSafe.Green;
      bool isBluePossible = game.Sets.Select(_ => _.Blue).Max() <= configurationSafe.Blue;


      return isRedPossible && isGreenPossible && isBluePossible;
    };

    return (games ?? Enumerable.Empty<Game>())
      .Where(_ => _ != null)
      .Where(isGamePossible.Invoke)
      .Select(_ => _.Id)
      .Sum();
  }

    public static int SumOfPowerOfTheseSets(IEnumerable<Game> games)
  {
    Func<Game, int> calculateGamePower = game =>
    {
      int redMin = game.Sets.Select(_ => _.Red).Max();
      int greenMin = game.Sets.Select(_ => _.Green).Max();
      int blueMin = game.Sets.Select(_ => _.Blue).Max();

      return redMin * greenMin * blueMin;
    };

    return (games ?? Enumerable.Empty<Game>())
      .Where(_ => _ != null)
      .Select(calculateGamePower.Invoke)
      .Sum();
  }

  public record Game
  {
    public required int Id { get; init; }
    public required IEnumerable<Set> Sets { get; init; }
  }

  public record Set
  {
    public required int Red { get; init; }
    public required int Blue { get; init; }
    public required int Green { get; init; }
  }


  public record Configuration
  {
    public required int Red { get; init; }
    public required int Blue { get; init; }
    public required int Green { get; init; }

  }

}
