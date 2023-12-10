using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using TinyFp;

namespace SeedMapper;

public class SeedMapper
{

  private IEnumerable<long> seeds;

  private IEnumerable<Range> seedToSoil;
  private IEnumerable<Range> soilToFertilizer;
  private IEnumerable<Range> fertilizerToWater;
  private IEnumerable<Range> waterToLight;
  private IEnumerable<Range> lightToTemperature;
  private IEnumerable<Range> temperatureToHumidity;
  private IEnumerable<Range> humidityToLocation;




  public SeedMapper(string filename)
  {
    seedToSoil = [];
    soilToFertilizer = [];
    fertilizerToWater = [];
    waterToLight = [];
    lightToTemperature = [];
    temperatureToHumidity = [];
    humidityToLocation = [];


    loadData(File.ReadAllLines(filename));

  }

  private (IEnumerable<Range>, int) parseMap(IEnumerable<Range> ranges, int index, IEnumerable<string> lines)
  {
    var line = lines.ElementAtOrDefault(index);

    if (string.IsNullOrWhiteSpace(line)) return (ranges, index);

    var elements = line.Split(" ").Select(_ => _.Trim()).Select(long.Parse);
    if (elements.Count() < 3)
    {
      Console.WriteLine($"Skipping line due to wrong number of elements {line.Count()}-> {line}");
    }

    var destination = elements.ElementAt(0);
    var source = elements.ElementAt(1);
    var gap = elements.ElementAt(2);

    Console.WriteLine($"{source} -> {destination} gap {gap}");
    var range = new Range
    {
      source = source,
      destination = destination,
      gap = gap
    };

    return parseMap(ranges.Append(range), index += 1, lines);
  }


  private void loadData(IEnumerable<string> input)
  {
    seeds = input.First().Split(":")[1].Split(" ").Select(_ => _.Trim()).Where(_ => !string.IsNullOrWhiteSpace(_)).Select(long.Parse);

    Func<IEnumerable<Range>> target = () => seedToSoil;

    for (int i = 1; i < input.Count(); i += 1)
    {
      var line = input.ElementAt(i);
      if (line.Equals("seed-to-soil map:"))
      {
        (seedToSoil, i) = parseMap([], i += 1, input);
      }
      else if (line.Equals("soil-to-fertilizer map:"))
      {
        (soilToFertilizer, i) = parseMap([], i += 1, input);
      }
      else if (line.Equals("fertilizer-to-water map:"))
      {
        (fertilizerToWater, i) = parseMap([], i += 1, input);
      }
      else if (line.Equals("water-to-light map:"))
      {
        (waterToLight, i) = parseMap([], i += 1, input);
      }
      else if (line.Equals("light-to-temperature map:"))
      {
        (lightToTemperature, i) = parseMap([], i += 1, input);
      }
      else if (line.Equals("temperature-to-humidity map:"))
      {
        (temperatureToHumidity, i) = parseMap([], i += 1, input);
      }
      else if (line.Equals("humidity-to-location map:"))
      {
        (humidityToLocation, i) = parseMap([], i += 1, input);
      }
      else if (string.IsNullOrWhiteSpace(line))
      {
        Console.WriteLine("Skipping whitespace..");
      }
      else
      {
        Console.WriteLine($"Not expected! {line}");
      }
    }
  }

  public long SeedToLocation(long seed)
  {
    Func<IEnumerable<Range>, long, long> findMatch = (ranges, input) =>
    {
      return ranges
                  .Where(_ => _.isIn(input))
                  .Select(_ => _.map(input))
                  .SingleOrDefault(input);
    };

    return Option<long>.Some(seed)
      .Map(_ => findMatch.Invoke(seedToSoil, _))
      .Map(_ => findMatch.Invoke(soilToFertilizer, _))
      .Map(_ => findMatch.Invoke(fertilizerToWater, _))
      .Map(_ => findMatch.Invoke(waterToLight, _))
      .Map(_ => findMatch.Invoke(lightToTemperature, _))
      .Map(_ => findMatch.Invoke(temperatureToHumidity, _))
      .Map(_ => findMatch.Invoke(humidityToLocation, _))
      .OrElse(-1);
  }


  public long MinLocationFromSeeds()
  {
    return seeds
      .Select(SeedToLocation)
      .Min();
  }

  public long MinLocationFromSeedsConsideredAsRanges()
  {
    var seedsSources = seeds.Where((_, index) => index % 2 == 0).ToList();
    var seedsDestinations = seeds.Where((_, index) => index % 2 != 0).ToList();

    var seedsAsRanges = seedsSources.Zip(seedsDestinations)
      .Select((_) => new Range
      {
        source = _.First,
        destination = _.First + _.Second,
        gap = _.Second
      }).ToList();


    Func<IEnumerable<Range>, App, App> findMatch = (ranges, input) =>
    {
      return ranges
                .Where(_ => _.isInReversed(input.current))
                .Select(_ => _.mapReversed(input.current))
                .Select(_ => new App { start = input.start, current = _})
                .FirstOrDefault(input);
    };

    Console.WriteLine(humidityToLocation.Count());

    Func<long, long> prlong = _ =>
    {
      Console.WriteLine(_);
      return _;
    };

    return Enumerable.Range(0, int.MaxValue)
      .Select(_ => new App { start = _, current = _})
      .Select(_ => findMatch.Invoke(humidityToLocation, _))
      .Select(_ => findMatch.Invoke(temperatureToHumidity, _))
      .Select(_ => findMatch.Invoke(lightToTemperature, _))
      .Select(_ => findMatch.Invoke(waterToLight, _))
      .Select(_ => findMatch.Invoke(fertilizerToWater, _))
      .Select(_ => findMatch.Invoke(soilToFertilizer, _))
      .Select(_ => findMatch.Invoke(seedToSoil, _))
      .Where(_ => seedsAsRanges.Where(seed => seed.isIn(_.current)).Any())
      .Select(_ => _.start)
      .First();
  }

  record App {
    public long start {get; init;}
    public long current {get; init;}
      
  }

  private class Range
  {
    public long source { get; init; }
    public long destination { get; init; }
    public long gap { get; init; }

    public bool isIn(long input)
    {
      var result = input >= source && input <= source + gap - 1;
      //Console.WriteLine($"isIn: Looking if {input} is into {source} -> {source + gap - 1} = {result}");


      return result;
    }

    public bool isInReversed(long input)
    {
      var result = input >= destination && input <= destination + gap - 1;
      //Console.WriteLine($"Looking if {input} is into {destination} -> {destination + gap - 1} = {result}");


      return result;
    }

    public long map(long input)
    {
      if (!isIn(input)) return input;

      return destination + (input - source);
    }

    public long mapReversed(long input)
    {
      if (!isInReversed(input)) return input;

      return source + (input - destination);
    }

    public IEnumerable<int> toIterator()
    {
      return Enumerable.Range((int)destination, (int)gap);
    }
  }
}
