using LinqTest;

namespace TestProject1
{
    public class Where
    {
       

        [Fact]
        public void Where_ReturnEvenNumbers()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6 };

            List<int> results = LinQ.From(numbers).Where(x => x % 2 == 0).ToList();

            Assert.Equal(3, results.Count);
        }

        [Fact]
        public void Where_ReturnsEmptyList()
        {
            List<int> numbers = new List<int> { 1, 3, 5 };

            List<int> results = LinQ.From(numbers).Where(x => x % 2 == 0).ToList();

            Assert.Empty(results);
        }





    }
}