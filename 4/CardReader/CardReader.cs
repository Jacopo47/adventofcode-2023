namespace CardReader;

public class CardReader
{


  readonly static Func<string, IEnumerable<string>> readFile = File.ReadLines;

  static Func<Card, IEnumerable<int>> findIntersection = card =>
  {
    return card.winning.Intersect(card.numbers);
  };

  static Func<IEnumerable<int>, int> calculatePoints = numbers =>
  {
    return (int)(Math.Pow(2, numbers.Count()) / 2);
  };

  public static IEnumerable<Card> loadCalibrationsFromFile(string path)
  {

    Func<string, Card> parse = input =>
    {
      var winningAsString = string.Join("", input
                .SkipWhile(_ => _ != ':')
                .Skip(1)
                .TakeWhile(_ => _ != '|'))
                .Trim();

      var yourNumbersAsString = string.Join("", input
                .SkipWhile(_ => _ != '|')
                .Skip(1))
                .Trim();

      IEnumerable<int> winningNumbers = winningAsString.Split(" ")
                            .Select(_ => _.Trim())
                            .Where(_ => !string.IsNullOrEmpty(_))
                            .Select(_ => int.Parse(_));

      IEnumerable<int> yourNumbers = yourNumbersAsString.Split(" ")
                      .Select(_ => _.Trim())
                      .Where(_ => !string.IsNullOrEmpty(_))
                      .Select(_ => int.Parse(_));


      return new Card
      {
        winning = winningNumbers,
        numbers = yourNumbers
      };
    };

    return readFile.Invoke(path)
      .Select(parse.Invoke);
  }

  public static int findYourWins(IEnumerable<Card> input)
  {
    Func<Card, IEnumerable<int>> findIntersection = card =>
    {
      return card.winning.Intersect(card.numbers);
    };

    Func<IEnumerable<int>, int> calculatePoints = numbers =>
    {
      return (int)(Math.Pow(2, numbers.Count()) / 2);
    };

    return (input ?? Enumerable.Empty<Card>())
      .Select(findIntersection.Invoke)
      .Select(calculatePoints.Invoke)
      .Sum();
  }

  public static int findYourWinsWithCopiesAsBonus(IEnumerable<Card> input)
  {
    var accumulator = 0;
    var bonuses = new Dictionary<int, int>();

    Action<int, int> populateDictionaryOrIncrement = (a, b) =>
    {
      bonuses[a + b] = bonuses.GetValueOrDefault(a + b, 0) + 1;
    };

    for (int i = 0; i < input.Count(); i++)
    {
      var bonus = bonuses.GetValueOrDefault(i, 0) + 1;
      var intersection = findIntersection.Invoke(input.ElementAt(i));
      accumulator += bonus;


      Enumerable.Range(1, bonus).ToList()
        .ForEach(ignore => Enumerable.Range(1, intersection.Count())
                                            .ToList()
                                            .ForEach(_ => populateDictionaryOrIncrement.Invoke(_, i)));

    }

    return accumulator;
  }


  public record Card
  {
    public IEnumerable<int> winning { get; init; }
    public IEnumerable<int> numbers { get; init; }
  }

}
