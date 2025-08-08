using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentManager.MVC.ViewModel
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        public string? FullName { get; set; }
        [Display(Name = "Tuổi")]
        [Range(18, 30, ErrorMessage = "Tuổi phải từ 18 đến 30")]
        public int Age { get; set; }
        [Display(Name = "Lớp")]
        public int ClassId { get; set; }
        //Property hiển thị tên lớp
        [Display(Name = "Tên lớp")]
        public string? ClassName { get; set; }

    }
}
