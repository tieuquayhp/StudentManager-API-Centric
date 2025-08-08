using System.ComponentModel.DataAnnotations;

namespace StudentManager.API.DTOs
{
    public class StudentCreateUpdateDto
    {
        public int Id { get; set; } // Cần cho việc Update
        [Required]
        public string FullName { get; set; }
        public int Age { get; set; }
        public int ClassId { get; set; }
    }
}
