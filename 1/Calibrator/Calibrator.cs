namespace Calibrator;

public class Calibrator
{
  readonly Func<string, IEnumerable<string>> readFile = File.ReadLines;


  public IEnumerable<string> loadCalibrationsFromFile(string path)
  {
    return readFile.Invoke(path);
  }

  public int sumOfCalibrations(IEnumerable<string> input)
  {
    return (input ?? Enumerable.Empty<string>())
      .Where(_ => _ != null)
      .Select(chars => chars.Where(Char.IsDigit))
      .Select(_ => _.ToList())
      .Where(_ => _.Count > 0)
      .Select(_ => $"{_.First()}{_.Last()}")
      .Select(int.Parse)
      .Sum();
  }
}
