using StudentsCRM.Data;
using StudentsCRM.Models;
using Microsoft.EntityFrameworkCore;
using StudentsCRM.Interfaces;

namespace StudentsCRM.Repository;

// CRUD
// Create - Создать
// Read - Читать
// Update - Обновить
// Delete - Удалить

public class StudentsRepository(StudentsDbContext db)
{
    private readonly StudentsDbContext _db = db;

    #region Студенты
    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await _db.Students
            .AsNoTracking()
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .ToListAsync();
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        return await _db.Students
            .AsNoTracking()
            .Include(s => s.Grades)
            .Include(s => s.Courses)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddStudentAsync(Student student)
    {
        await _db.Students.AddAsync(student);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateStudentAsync(Student student)
    {
        _db.Students.Update(student);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteStudentByIdAsync(int id)
    {
        var student = await GetStudentByIdAsync(id);
        if (student != null)
        {
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
        }
    }
    #endregion

    #region Курсы
    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return await _db.Courses
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        return await _db.Courses
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCourseAsync(Course course)
    {
        await _db.Courses.AddAsync(course);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateCourseAsync(Course course)
    {
        _db.Courses.Update(course);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteCourseByIdAsync(int id)
    {
        var course = await GetCourseByIdAsync(id);
        if (course != null)
        {
            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();
        }
    }

    #endregion

    #region Оценки
    public async Task AddGradeAsync(Grade grade)
    {
        await _db.Grades.AddAsync(grade);
        await _db.SaveChangesAsync();
    }

    #endregion

    #region Связи

    public async Task EnrollStudentInCourseAsync(int studentId, int courseId)
    {
        var student = await _db.Students
                        .Include(s => s.Courses)
                        .FirstOrDefaultAsync(s => s.Id == studentId);

        var course = await _db.Courses.FirstOrDefaultAsync(courseId);

        if (student is not null && course is not null)
        {
            student.Courses.Add(course);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Student>> GetStudentsWithCoursesAsync()
    {
        return await _db.Students
                    .AsNoTracking()
                    .Include(s => s.Courses)
                    .Include(s => s.Grades)
                    .OrderBy(s => s.LastName)
                    .ToListAsync();
    }

    public async Task<List<Course>> GetCoursesWithStudentsAsync()
    {
        return await _db.Courses
                    .Include(c => c.Students)
                    .OrderBy(c => c.Name)
                    .ToListAsync();
    }
    #endregion
}

