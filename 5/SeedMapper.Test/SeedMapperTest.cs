namespace SeedMapper.Test;

public class SeedMapperTest
{
    [Fact]
    public void PartOne_ResolveExample()
    {

        var test = new SeedMapper("res/example.txt");

        Assert.Equal(82, test.SeedToLocation(79));
        Assert.Equal(43, test.SeedToLocation(14));
        Assert.Equal(86, test.SeedToLocation(55));
        Assert.Equal(35, test.SeedToLocation(13));

        Assert.Equal(35, test.MinLocationFromSeeds());
    }


    [Fact]
    public void PartOne_ResolveExercise()
    {
        var test = new SeedMapper("res/input.txt");
        Assert.Equal(51752125, test.MinLocationFromSeeds());
    }

    [Fact]
    public void PartTwo_ResolveExample()
    {

        var test = new SeedMapper("res/example.txt");

        Assert.Equal(46, test.MinLocationFromSeedsConsideredAsRanges());
    }


    [Fact]
    public void PartTwo_ResolveExercise()
    {
        var test = new SeedMapper("res/input.txt");
        Assert.Equal(12634632, test.MinLocationFromSeedsConsideredAsRanges());
    }
}