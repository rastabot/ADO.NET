using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;


namespace MyFirstPOCO
{
    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Courses> Courses { get; set; }
    }
}
