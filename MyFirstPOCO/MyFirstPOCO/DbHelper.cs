using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstPOCO
{
    public class DbHelper
    {
        readonly StudentContext dbContext = new StudentContext();

        public List<Student> GetStudents()
        {
            return dbContext.Students.ToList();
        }
    }
}
