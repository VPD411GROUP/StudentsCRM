using StudentsCRM.Models;

namespace StudentsCRM.Interfaces
{
    public interface IStudentsRepository
    {
        Task AddCourseAsync(Course course);
        Task AddGradeAsync(Grade grade);
        Task AddStudentAsync(Student student);
        Task DeleteCourseByIdAsync(int id);
        Task DeleteStudentByIdAsync(int id);
        Task EnrollStudentInCourseAsync(int studentId, int courseId);
        Task<List<Course>> GetAllCoursesAsync();
        Task<List<Student>> GetAllStudentsAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        Task<List<Course>> GetCoursesWithStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<List<Student>> GetStudentsWithDetailsAsync();
        Task UpdateCourseAsync(Course course);
        Task UpdateStudentAsync(Student student);
    }
}