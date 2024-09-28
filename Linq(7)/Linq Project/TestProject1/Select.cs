using LinqTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class Select
    {
        public static List<Student> GetSampleStudents()
        {
            return new List<Student>
        {
                new Student("John Doe", 22, "Male", "123 Main St", 3.75),
                new Student("Jane Smith", 20, "Female", "456 Elm St", 3.85),
                new Student("Tom Brown", 21, "Male", "789 Oak St", 3.95),
                new Student("Emily Davis", 21, "Female", "321 Pine St", 2.85),
                new Student("Michael Johnson", 23, "Male", "654 Maple St", 3.65)
        };
        }


        [Fact]
        public void Select_WorkCorrectly_WithValidData()
        {
            List<int> numbers = new List<int> { 1, 2, 3, };

            List<int> result = LinQ.From(numbers).Select(x => x * 2).ToList();

            List<int> expected = new List<int> { 2, 4, 6 };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Select_ReturnEmptyList_WhenSourceIsEmpty()
        {
            List<int> numbers = new List<int> {};

            List<int> result = LinQ.From(numbers).Select(x => x * 2).ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void Select_NullElementsInSource()
        {
            List<string> strings = new List<string> { "Hello", null, "World" };
            List<int> result= LinQ.From(strings).Select(s => s?.Length ?? 0).ToList();

            List<int> expected = new List<int> { 5, 0, 5 };

            Assert.Equal(expected, result);
        }
    }
}
