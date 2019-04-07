using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstPOCO
{
    public class Student
    {
        public int StudentId{ get; set; }
        public string StudentName{ get; set; }
        public int CourseId { get; set; }

        // Navigation property
        public virtual Courses Course { get; set; }
    }
}
