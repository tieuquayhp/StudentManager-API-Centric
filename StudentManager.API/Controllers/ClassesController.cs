using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManager.DAL.Data;
using StudentManager.API.DTOs;

namespace StudentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly StudentManagerDbContext _context;
        public ClassesController(StudentManagerDbContext context)
        {
            _context = context;
        }
        // GET: api/classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses()
        {
            var classes = await _context.Classes
                .Select(C => new ClassDto
                {
                    Id = C.Id,
                    ClassName = C.ClassName
                }).ToListAsync();
            return Ok(classes);
        }
        // GET: api/classes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassDto>> GetClass(int id)
        {
            var classes = await _context.Classes
                .Where(c => c.Id == id)
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    ClassName = c.ClassName
                })
                .FirstOrDefaultAsync();
            if (classes == null)
            {
                return NotFound("Class not found");
            }
            return Ok(classes);
        }
        // POST: api/classes
        [HttpPost]
        public async Task<ActionResult<ClassDto>> PostClass([FromBody] ClassCreateUpdateDto classDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newClass = new Class
            { ClassName = classDto.ClassName };
            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();
            var resultDto = new ClassDto
            {
                Id = newClass.Id,
                ClassName = newClass.ClassName
            };
            return CreatedAtAction(nameof(GetClass), new { id = newClass.Id }, resultDto);
        }
        // PUT: api/classes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(int id, [FromBody] ClassCreateUpdateDto classDto)
        {
            if (id != classDto.Id)
            {
                return BadRequest("Class ID mismatch");
            }
           var classToUpdate = await _context.Classes
                .FirstOrDefaultAsync(c => c.Id == id);
            if (classToUpdate == null)
            {
                return NotFound("Class not found");
            }
            classToUpdate.ClassName = classDto.ClassName;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClassExists(id))
                {
                    return NotFound("Class not found");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }  

        // DELETE: api/classes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var classToDelete = await _context.Classes
                .FindAsync(id);
            if (classToDelete == null)
            {
                return NotFound("Class not found");
            }
            _context.Classes.Remove(classToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private async Task<bool> ClassExists(int id)
        {
            return _context.Classes.Any(c => c.Id == id);
        }
    }
}
