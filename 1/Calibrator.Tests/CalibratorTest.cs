namespace Calibrator.Test;

public class CalibratorTest
{
    [Fact]
    public void WorksFromStaticInputList()
    {
        var input = new List<string>() { "1abc2", "pqr3stu8vwx", "a1b2c3d4e5f", "treb7uchet" };


        Assert.Equal(142, new Calibrator().sumOfCalibrations(input));
    }

    [Fact]
    public void IsNullSafe()
    {
        var input = new List<string>() { "1abc2", "pqr3stu8vwx", "a1b2c3d4e5f", "treb7uchet", null };


        Assert.Equal(0, new Calibrator().sumOfCalibrations(null));
        Assert.Equal(142, new Calibrator().sumOfCalibrations(input));
    }

    [Fact]
    public void WorksFromStaticInputList_3()
    {
        var calibrator = new Calibrator();

        Assert.Equal(55607, calibrator.sumOfCalibrations(calibrator.loadCalibrationsFromFile("res/input.txt")));
    }
}
