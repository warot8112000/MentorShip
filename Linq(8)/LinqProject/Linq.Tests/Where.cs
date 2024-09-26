namespace Linq.Tests;

public class Where
{
    [Fact]
    public void Where_FilterEvenNumbers()
    {
        List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        List<int> result = LinQ.From(numbers).Where(x => x % 2 == 0).ToList();

        List<int> expected = new() { 2, 4, 6, 8, 10 };
        Assert.Equal(expected, result);
    }
    
}