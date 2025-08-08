using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager.DAL.Data
{
    public  class Student
    {
        public int Id { get; set; }
        [Required]
        public string? FullName { get; set; }
        public int Age { get; set; }
        //Thêm khoá phụ
        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
    }
}
