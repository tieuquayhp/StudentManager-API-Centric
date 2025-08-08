using System.ComponentModel.DataAnnotations;

namespace StudentManager.MVC.ViewModel
{
    public class ClassViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Tên lớp")]
        [Required(ErrorMessage = "Tên lớp là bắt buộc")]
        public string? ClassName { get; set; }
    }
}
