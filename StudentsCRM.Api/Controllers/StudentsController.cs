using StudentsCRM.Interfaces;
using StudentsCRM.Api.DTO;
using StudentsCRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace StudentsCRM.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StudentsController : Controller
{
    private readonly IStudentsRepository _repo;

    public StudentsController(IStudentsRepository repo)
    {
        _repo = repo;
    }

    // GET: api/students
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents()
    {
        var students = await _repo.GetAllStudentsAsync();
        students
            .Select(s => new StudentDTO
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                BirthDate = s.BirthDate
            }).ToList();

        return Ok(students);
    }

    //GET: api/students/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDTO>> GetStudent(int id)
    {
        var student = await _repo.GetStudentByIdAsync(id);

        if (student is null)
            return NotFound();

        var studentDto = new StudentDTO
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            BirthDate = student.BirthDate
        };

        return Ok(studentDto);
    }

    //POST: api/students
    [HttpPost]
    public async Task <ActionResult<StudentDTO>> CreateStudent(CreateStudentDto dto)
    {
        var student = new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            BirthDate = dto.BirthDate
        };

        await _repo.AddStudentAsync(student);

        var studentDto = new StudentDTO
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
        };

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, studentDto);
    }

    //PUT: api/students/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto dto)
    {
        var student = await _repo.GetStudentByIdAsync(id);
        if (student is null) return NotFound();

        student.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.Email = dto.Email;

        await _repo.UpdateStudentAsync(student);
        return NoContent();
    }

    //DELETE: api/students/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        var student = await _repo.GetStudentByIdAsync(id);
        if (student is null) return NotFound();

        await _repo.DeleteStudentByIdAsync(id);

        return NoContent();
    }
}
