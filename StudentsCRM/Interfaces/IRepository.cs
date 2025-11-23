using StudentsCRM.Models;

namespace StudentsCRM.Interfaces
{
    public interface IRepository
    {
        void Add(string firstName, string lastName, DateTime birthDate, string email);
        void Delete(int id);
        List<Student> GetAll();
        Student? GetById(int id);
        void Update(int id, string firstName, string lastName, DateTime birthDate, string email);
    }
}