namespace Linq.Tests;

public class Demo
{
    [Fact]
    public void Where_FilterEvenNumbers()
    {
        List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        List<int> result = LinQ.From(numbers).Where(x => x % 2 == 0).ToList();

        List<int> expected = new() { 2, 4, 6, 8, 10 };
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void Select_SquareNumbers()
    {
        List<a> numbers = new() { 1, 2, 3, 4, 5 };

        List<int> result = LinQ.From(numbers).Select(x => x * x).ToList();

        List<int> expected = new() { 1, 4, 9, 16, 25 };
        Assert.Equal(expected, result);
    }
}