namespace RaceManager.Test;

public class RaceManagerTest
{
    [Fact]
    public void PartOne_Example()
    {
        var raceManager = new RaceManager();
        var input = raceManager.readFile("res/example.txt");
        Assert.Equal(288, raceManager.calculateWaysToBeatTheRecords(input, (a, b, c, d) => RaceManager.howManyWaysToBeatTheRecords(a, b, c, d)));
    }
    
    [Fact]
    public void PartOne_Exercise()
    {
        var raceManager = new RaceManager();
        var input = raceManager.readFile("res/input.txt");
        Assert.Equal(131376, raceManager.calculateWaysToBeatTheRecords(input, (a, b, c, d) => RaceManager.howManyWaysToBeatTheRecords(a, b, c, d)));
    }

    [Fact]
    public void PartTwo_Example()
    {
        var raceManager = new RaceManager();
        var input = raceManager.readFileAsSingleRace("res/example.txt");
        Assert.Equal(71503, raceManager.calculateWaysToBeatTheRecords(input, (a, b, c, d) => RaceManager.howManyWaysToBeatTheRecordsWithMath(a, b, c, d)));
    }
    
    [Fact]
    public void PartTwo_Exercise()
    {
        var raceManager = new RaceManager();
        var input = raceManager.readFileAsSingleRace("res/input.txt");
        Assert.Equal(34123437, raceManager.calculateWaysToBeatTheRecords(input, (a, b, c, d) => RaceManager.howManyWaysToBeatTheRecordsWithMath(a, b, c, d)));
    }
}