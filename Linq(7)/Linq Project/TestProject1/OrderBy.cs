using LinqTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class OrderBy
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
        public void OrderBy_ReturnsAscendingList()
        {
            List<int> numbers = new() { 5, 2, 8, 3, 1 };
            List<int> result = LinQ.From(numbers).OrderBy(x => x).ToList();
            List<int> expected = new() { 1, 2, 3, 5, 8 };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OrderByDescending_ReturnsDescendingList()
        {
            List<int> numbers = new() { 5, 2, 8, 3, 1 };
            List<int> result = LinQ.From(numbers).OrderByDescending(x => x).ToList();
            List<int> expected = new() { 8, 5, 3, 2, 1 };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OrderBy_ReturnsSortedStudentList()
        {
            List<Student> students = GetSampleStudents();
            List<Student> result = LinQ.From(students).OrderBy(x => x.Age).ToList();
            List<Student> expected = new List<Student>
            {
                new Student("Tom Brown", 19, "Male", "789 Oak St", 3.95),   
                new Student("John Doe", 20, "Male", "123 Main St", 3.75),   
                new Student("Emily Davis", 21, "Female", "321 Pine St", 2.85), 
                new Student("Jane Smith", 22, "Female", "456 Elm St", 3.85),  
                new Student("Michael Johnson", 23, "Male", "654 Maple St", 3.65) 
            };

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i].Age >= result[i - 1].Age, "Students are not sorted by age.");
            }
        }

        [Fact]
        public void OrderBy_EmptyList_ReturnsEmptyList()
        {
            
            List<Student> students = new List<Student>();

            List<Student> result = LinQ.From(students).OrderBy(x => x.Age).ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void OrderBy_AlreadySorted_ReturnsSameList()
        {
            // Arrange
            List<Student> students = new List<Student>
        {
            new Student("Jane Smith", 20, "Female", "456 Elm St", 3.85),
            new Student("Tom Brown", 21, "Male", "789 Oak St", 3.95),
            new Student("John Doe", 22, "Male", "123 Main St", 3.75)
        };

            // Act
            List<Student> result = LinQ.From(students).OrderBy(x => x.Age).ToList();

            // Assert
            Assert.Equal(students, result);
        }

        

    }
}
