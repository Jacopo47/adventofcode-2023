namespace GameTable.Tests;

public class GameTableTest
{

    [Fact]
    public void ParseLineToGame()
    {
        Assert.Equal(3, GameTable.ParseLineToGame("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red").Id);
        Assert.Equal(2, GameTable.ParseLineToGame("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green  ").Sets.Count());

    }

    [Fact]
    public void ParseSet()
    {
        Assert.Equal(20, GameTable.parseSet.Invoke("8 green, 6 blue, 20 red").Red);
        Assert.Equal(8, GameTable.parseSet.Invoke("8 green, 6 blue, 20 red").Green);
        Assert.Equal(6, GameTable.parseSet.Invoke("8 green, 6 blue, 20 red").Blue);
        Assert.Equal(8, GameTable.parseSet.Invoke("8 green, 20 red").Green);
        Assert.Equal(20, GameTable.parseSet.Invoke("20 red").Red);



    }

    [Fact]
    public void PartOne_ResolvesExample()
    {
        var input = GameTable.loadCalibrationsFromFile("res/example.txt");

        Assert.Equal(8, GameTable.SumOfValidGameIds(input, new GameTable.Configuration {
            Red = 12, Green = 13, Blue = 14
        }));
    }

    [Fact]
    public void IsNullSafe()
    {
        Assert.Equal(0, GameTable.SumOfValidGameIds(null, null));
    }

    [Fact]
    public void PartOne_ResolvesExercise()
    {
        var input = GameTable.loadCalibrationsFromFile("res/input.txt");

        Assert.Equal(2439, GameTable.SumOfValidGameIds(input, new GameTable.Configuration {
            Red = 12, Green = 13, Blue = 14
        }));
    }

    [Fact]
    public void PartTwo_ResolvesExample()
    {
        var input = GameTable.loadCalibrationsFromFile("res/example-2.txt");


        Assert.Equal(2286, GameTable.SumOfPowerOfTheseSets(input));
    }

    [Fact]
    public void PartTwo_ResolvesExercise()
    {
        var input = GameTable.loadCalibrationsFromFile("res/input.txt");


        Assert.Equal(63711, GameTable.SumOfPowerOfTheseSets(input));
    }

}