using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager.DAL.Data
{
    public class StudentManagerDbContext:DbContext
    {
        public StudentManagerDbContext(DbContextOptions<StudentManagerDbContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }   
    }
}
