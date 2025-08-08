using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager.DAL.Data
{
    public class Class
    {
        public int Id { get; set; }
        [Required]
        public string ClassName { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    }
}
