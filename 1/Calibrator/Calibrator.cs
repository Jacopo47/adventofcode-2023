namespace Calibrator;

public class Calibrator
{

  static readonly IEnumerable<KeyValuePair<string, string>> SPELLED_DIGITS = new Dictionary<string, string>() {
    { "one" , "1" },
    { "two" , "2" },
    { "three" , "3" },
    { "four" , "4" },
    { "five" , "5" },
    { "six" , "6" },
    { "seven" , "7" },
    { "eight" , "8" },
    { "nine" , "9" }
  }.ToList();
  static readonly Func<string, string> DO_NOT_REPLACE_SPELLED_DIGITS = _ => _;



  readonly Func<string, IEnumerable<string>> readFile = File.ReadLines;


  public IEnumerable<string> loadCalibrationsFromFile(string path)
  {
    return readFile.Invoke(path);
  }

  public static int SumOfCalibrations(IEnumerable<string> input, Func<string, string>? unsafeReplaceSpelledDigits = null)
  {
    var replaceSpelledDigits = unsafeReplaceSpelledDigits ?? DO_NOT_REPLACE_SPELLED_DIGITS;

    return (input ?? Enumerable.Empty<string>())
      .Where(_ => _ != null)
      .Select(replaceSpelledDigits)
      .Select(chars => chars.Where(Char.IsDigit))
      .Select(_ => _.ToList())
      .Where(_ => _.Count > 0)
      .Select(_ => $"{_.First()}{_.Last()}")
      .Select(int.Parse)
      .Sum();
  }

  public static string ReplaceSpelledDigits(string input)
  {
    return ReplaceSpelledDigits(input, SPELLED_DIGITS);;
  }

  public static string ReplaceSpelledDigits(string input, IEnumerable<KeyValuePair<string, string>>? toReplace = null)
  {
    var toReplaceSafe = toReplace ?? SPELLED_DIGITS;

    if (!toReplaceSafe.Any()) return input;

    var element = toReplaceSafe.First();
    /* While replacing I'm keeping the "key" value so the spelled value in order to not lose information.
      In case the last letter of a number is the first of the next one, like:
       - eightwothree
       - xtwone3four
    */
    var inputWithAppliedReplace = input.Replace(element.Key, $"{element.Key}{element.Value}{element.Key}");

    if (toReplaceSafe.Count() == 1) return inputWithAppliedReplace;

    return ReplaceSpelledDigits(inputWithAppliedReplace, toReplaceSafe.Skip(1));
  }
}
