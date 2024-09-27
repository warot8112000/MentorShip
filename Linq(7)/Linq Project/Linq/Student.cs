using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqTest
{
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public double AverageGrade { get; set; }

        public Student(string name, int age, string gender, string address, double averageGrade)
        {
            Name = name;
            Age = age;
            Gender = gender;
            Address = address;
            AverageGrade = averageGrade;
        }
    }
}
