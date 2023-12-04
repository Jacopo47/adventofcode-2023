namespace GondolaLift.Test;

public class UnitTest1
{
    GondolaEngine engine = new GondolaEngine();

    [Fact]
    public void PartOne_ResolveExample()
    {

        var input = GondolaEngine.loadInputFile("res/example-1.txt");

        Assert.Equal(4361, engine.discoverAndSumPartNumbers(input));
    }

        [Fact]
    public void PartOne_ResolveExercise()
    {

        var input = GondolaEngine.loadInputFile("res/input.txt");

        Assert.Equal(527369, engine.discoverAndSumPartNumbers(input));
    }
}