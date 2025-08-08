namespace StudentManager.API.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public int Age { get; set; }
        public int ClassId { get; set; }
        public string? ClassName { get; set; }

    }
}
