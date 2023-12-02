namespace Calibrator.Test;

public class CalibratorTest
{
    [Fact]
    public void PartOne_ResolvesExample()
    {
        var input = new List<string>() { "1abc2", "pqr3stu8vwx", "a1b2c3d4e5f", "treb7uchet" };


        Assert.Equal(142, Calibrator.SumOfCalibrations(input));
    }

    [Fact]
    public void IsNullSafe()
    {
        var input = new List<string>() { "1abc2", "pqr3stu8vwx", "a1b2c3d4e5f", "treb7uchet", null };


        Assert.Equal(0, Calibrator.SumOfCalibrations(null));
        Assert.Equal(142, Calibrator.SumOfCalibrations(input));
    }

    [Fact]
    public void PartOne_ResolvesExercise()
    {
        var calibrator = new Calibrator();

        Assert.Equal(55607, Calibrator.SumOfCalibrations(calibrator.loadCalibrationsFromFile("res/input.txt")));
    }

    [Fact]
    public void PartTwo_ResolvesExample()
    {
        var input = new List<string>() { "two1nine", "eightwothree", "abcone2threexyz", "xtwone3four", "4nineeightseven2", "zoneight234", "7pqrstsixteen" };

        Assert.Equal(281, Calibrator.SumOfCalibrations(input, Calibrator.ReplaceSpelledDigits));


        Assert.Equal(12, Calibrator.SumOfCalibrations(new List<string>() {"oneeighttwo"}, Calibrator.ReplaceSpelledDigits));
        Assert.Equal(12, Calibrator.SumOfCalibrations(new List<string>() {"oneightwo"}, Calibrator.ReplaceSpelledDigits));

        Assert.Equal(18, Calibrator.SumOfCalibrations(new List<string>() {"oneight"}, Calibrator.ReplaceSpelledDigits));

  }

    [Fact]
    public void PartTwo_ResolvesExercise()
    {
        var calibrator = new Calibrator();

        Assert.Equal(55291, Calibrator.SumOfCalibrations(calibrator.loadCalibrationsFromFile("res/input.txt"), Calibrator.ReplaceSpelledDigits));
    }
}
