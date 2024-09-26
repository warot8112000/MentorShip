namespace LinqT.Tests;

public class UnitTest1
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
    public void Order()
    {
        List<int> numbers = new() { 5, 2, 8, 3, 1 };
        List<int> result = LinQ.From(numbers).OrderBy(x => x).ToList();
        List<int> expected = new() { 1, 2, 3, 5, 8 };
        Assert.Equal(expected, result);
    }
}