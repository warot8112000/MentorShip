namespace Linq.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<int> result = LinQ.From(numbers).Where(x => x % 2 == 0).ToList();
    }
}