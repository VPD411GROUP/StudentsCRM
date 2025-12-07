using StudentsCRM.Models;

namespace StudentsCRM.Interfaces
{
    public interface IStudentsRepository
    {
        Task AddAsync(Student student);
        Task DeleteByIdAsync(int id);
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task UpdateAsync(Student student);
    }
}