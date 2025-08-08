using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManager.DAL.Data;
using StudentManager.API.DTOs;

namespace StudentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentManagerDbContext _context;
        public StudentsController(StudentManagerDbContext context)
        {
            _context = context;
        }
        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            // Dùng .Select() để chiếu từ Entity sang DTO
            // Cách này rất hiệu quả, EF Core chỉ query đúng những cột cần thiết
            var students = await _context.Students
                .Include(s => s.Class)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    Age = s.Age,
                    ClassId = s.ClassId,
                    ClassName = s.Class != null ? s.Class.ClassName : null
                })
                .ToListAsync();
            return Ok(students);
        }
        // GET: api/students/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _context.Students
               .Where(s=>s.Id == id)
               .Include(s => s.Class)
               .Select(s=> new StudentDto
               {
                   Id = s.Id,
                   FullName = s.FullName,
                   Age = s.Age,
                   ClassId = s.ClassId,
                   ClassName = s.Class != null ? s.Class.ClassName : null
               })
               .FirstOrDefaultAsync();
            if (student == null)
            {
                return NotFound("Student not found");
            }
            return Ok(student);
        }
        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent([FromBody] StudentCreateUpdateDto studentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var student = new Student
            {
                FullName = studentDto.FullName,
                Age = studentDto.Age,
                ClassId = studentDto.ClassId
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            var resultDto= new StudentDto { 
                Id= student.Id,
                FullName = student.FullName,
                Age = student.Age
            };
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, resultDto);
        }
        // PUT: api/students/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, [FromBody] StudentCreateUpdateDto studenDto)
        {
            if (id != studenDto.Id)
            {
                return BadRequest("Student ID mismatch");
            }
            var studentToUpdate = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);
            if (studentToUpdate == null)
                {
                return NotFound("Student not found");
            }
            studentToUpdate.FullName = studenDto.FullName;  
            studentToUpdate.Age = studenDto.Age;    
            studentToUpdate.ClassId = studenDto.ClassId;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound("Student not found");
                }
                else
                {
                    throw;
                }
            }
            return NoContent(); 
        }
        // DELETE: api/students/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
