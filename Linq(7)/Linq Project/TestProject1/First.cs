using LinqTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class First
    {
        [Fact]
        public void First_ReturnsFirstMatchingElement()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4 };
            var result = LinQ.From(numbers).First(x => x % 2 == 0);
            Assert.Equal(2, result);
        }

        [Fact]
        public void First_ThrowsInvalidOperationException()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            Assert.Throws<InvalidOperationException>(() => LinQ.From(numbers).First(x => x > 10));
        }
    }
}
