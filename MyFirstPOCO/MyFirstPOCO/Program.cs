using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstPOCO
{
    class Program
    {
        static void Main(string[] args)
        {
            Courses cObj = new Courses
            {
                CourseName = "Test course",
                Students = new List<Student>
                {
                    new Student() { StudentName = "Stud1"},
                    new Student() { StudentName = "Stud2"},
                }
            };
        }
    }
}
