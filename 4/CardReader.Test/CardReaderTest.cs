namespace CardReader.Test;

public class CardReaderTest
{
    [Fact]
    public void PartOne_ResolvesExample()
    {

        var input = CardReader.loadCalibrationsFromFile("res/example.txt");

        Assert.Equal(13, CardReader.findYourWins(input));
    }

    [Fact]
    public void PartOne_ResolvesExercise()
    {

        var input = CardReader.loadCalibrationsFromFile("res/input.txt");

        Assert.Equal(13, CardReader.findYourWins(input));
    }

    [Fact]
    public void PartTwo_ResolvesExample()
    {

        var input = CardReader.loadCalibrationsFromFile("res/example.txt");

        Assert.Equal(30, CardReader.findYourWinsWithCopiesAsBonus(input));
    }

    [Fact]
    public void PartTwo_ResolvesExercise()
    {

        var input = CardReader.loadCalibrationsFromFile("res/input.txt");

        Assert.Equal(13, CardReader.findYourWinsWithCopiesAsBonus(input));
    }
}