using System.Text.RegularExpressions;

namespace RaceManager;

public class RaceManager
{

  readonly static Regex RACE_LINE = new Regex(@"(Time|Distance): (?<other>.*)");

  public IEnumerable<Race> readFileAsSingleRace(string filename) {
    return this.readFile(filename, _ => _.Replace(" ", ""));
  }

  public IEnumerable<Race> readFile(string filename, Func<string, string> manipulateValues = null) {
    Func<string, IEnumerable<long>> readStats = _ => {
      Match match = RACE_LINE.Match(_);

      if (!match.Success) {
        Console.WriteLine($"Unable to proper parse line {_}");
        return null;
      }

      Func<string, string> defaultBehavior = _ => _.Trim();
      var results = (manipulateValues ?? defaultBehavior)
            .Invoke(match.Groups["other"].Value)
            .Split(" ")
            .Where(_ => !string.IsNullOrWhiteSpace(_))
            .Select(long.Parse);

      return results;
    };

    var lines = File.ReadAllLines(filename);

    if (lines.Length != 2) return Enumerable.Empty<Race>();

    var times = readStats.Invoke(lines.First());
    var distances = readStats.Invoke(lines.Last());

    return times.Zip(distances, (time, distance) => (time, distance))
            .Select(_ => new Race { time = _.time, distance = _.distance });
  }


  public long calculateWaysToBeatTheRecords(IEnumerable<Race> races, Func<int, int, long, long, long> resolve) {
    return races
      .Select(race => resolve(0, 0, race.time, race.distance))
      .Aggregate(1, (x, y) => (int)(x * y));
  }

  public static long howManyWaysToBeatTheRecords(long accumulator, long speed, long timeLimit, long record) {
    long isWin = (speed * (timeLimit - speed)) >  record ? 1 : 0;

    if (speed == timeLimit) return accumulator + isWin;


    return howManyWaysToBeatTheRecords(accumulator + isWin, speed + 1, timeLimit, record);  
  }


  public static long howManyWaysToBeatTheRecordsWithMath(long accumulator, long speed, long timeLimit, long record) {
    var a = Math.Ceiling((timeLimit - Math.Sqrt(timeLimit * timeLimit - 4*record)) / 2);
    var b = Math.Floor((timeLimit + Math.Sqrt(timeLimit * timeLimit - 4*record)) / 2);

    Console.WriteLine($"{a} - {b}");

    return (long)(b - a + 1);
  }



  public record Race {
    public long time { get; init; }
    public long distance { get; init; }
  }

}
