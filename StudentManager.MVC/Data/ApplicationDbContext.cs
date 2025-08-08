using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManager.MVC.ViewModel;

namespace StudentManager.MVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StudentManager.MVC.ViewModel.StudentViewModel> StudentViewModel { get; set; } = default!;
        public DbSet<StudentManager.MVC.ViewModel.ClassViewModel> ClassViewModel { get; set; } = default!;
    }
}
