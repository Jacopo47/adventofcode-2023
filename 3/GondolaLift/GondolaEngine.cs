using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace GondolaLift;

public class GondolaEngine
{

  public static IEnumerable<string> loadInputFile(string path)
  {
    return File.ReadLines(path);
  }

  public int discoverAndSumPartNumbers(IEnumerable<string> lines) {
    var linesSafe = lines ?? Enumerable.Empty<string>();

    if (linesSafe.Count() == 0) return 0;
    if (linesSafe.Count() == 1) return discoverAndSumPartNumbers(0, null, linesSafe.First(), null, []);
    if (linesSafe.Count() == 2) return discoverAndSumPartNumbers(0, null, linesSafe.First(), linesSafe.Skip(1).First(), []);



    return discoverAndSumPartNumbers(0, null, linesSafe.First(), linesSafe.Skip(1).First(), linesSafe.Skip(2));
  }

  public int discoverAndSumPartNumbers(int accumulator, string? prev, string? curr, string? next, IEnumerable<string> tail) {
    Console.WriteLine($"prev: {prev}");
    Console.WriteLine($"curr: {curr}");
    Console.WriteLine($"next: {next}");

    if (curr == null) return accumulator;
    

    int rowCount = findNumbersAdjacentToSymbols(0, 0, [], false, prev != null ? [.. prev] : [.. curr], [.. curr], next != null ? [.. next] : null);
    Console.WriteLine(rowCount);

    var theNextAfterNext = tail.Any() ? tail.First() : null;

    return discoverAndSumPartNumbers(
        accumulator + rowCount,
        curr,
        next,
        theNextAfterNext,
        tail.Skip(1)
    );
  }

  public int findNumbersAdjacentToSymbols(int accumulator, int index, IEnumerable<char> numbersSequence, bool near, char[] prev, char[] curr, char[]? next) {
    if (index >= curr.Length) {
      //Console.WriteLine("I'm returning");
      var num = numbersSequence.Any() ? int.Parse(String.Concat(numbersSequence)) : 0;
      return near ? accumulator + num : accumulator;
    }
    char symbol = curr[index];
    bool isNear = isNearToSymbol(prev, curr, next, index);

    //Console.WriteLine($"{index}. {symbol} - has nears: {isNear}");


    if (Char.IsDigit(symbol)) {
      return findNumbersAdjacentToSymbols(accumulator, index+=1, numbersSequence.Append(symbol), near || isNear, prev, curr, next);
    }

    var number = numbersSequence.Any() ? int.Parse(String.Concat(numbersSequence)) : 0;
    //Console.WriteLine($"Summing {number} ? {near}");
    if (symbol == '.') {
      //Console.WriteLine("Skipping dot..");

      return findNumbersAdjacentToSymbols(near ? accumulator + number : accumulator, index+=1, [], false, prev, curr, next);
    }
    return findNumbersAdjacentToSymbols(near ? accumulator + number : accumulator, index+=1, [], isNear, prev, curr, next);
  }

  public bool isNearToSymbol(char[] prev, char[] curr, char[]? next, int index) {
    var prevAdjacentElements = prev.Skip(index - 1).Take(3);
    var nextAdjacentElements = (next ?? []).Skip(index - 1).Take(3);
    
    var prevElement = curr[index > 0 ? index - 1 : index];
    var nextElement = curr[index < curr.Length -1 ? index + 1 : index];


    var adjacents = prevAdjacentElements
      .Concat(nextAdjacentElements)
      .Append(prevElement)
      .Append(nextElement);

    Console.WriteLine($"[{string.Join(", ", adjacents.ToArray())}]");
    

    return adjacents
      .Where(_ => !(Char.IsDigit(_) || _ == '.'))
      .Any();
  }
}
