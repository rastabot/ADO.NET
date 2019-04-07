using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstPOCO
{
    public class Courses
    {
        public int CourseId { get; set; }
        public string CourseName{ get; set; }
        public string CourseDescription{ get; set; }

        // navigation property
        public virtual ICollection<Student> Students { get; set; }
    }
}
