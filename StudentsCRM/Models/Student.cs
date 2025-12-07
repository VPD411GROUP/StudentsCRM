namespace StudentsCRM.Models;

public class Student
{
    // Основные свойства
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; } 
    public DateTime BirthDate { get; set; }
    public required string Email { get; set; }

    // Ключи
    public List<Grade> Grades { get; set; } = [];
    public List<Course> Courses { get; set; } = [];

    public override string ToString()
    {
        return $"[{Id}] {FirstName} {LastName}, {BirthDate.ToShortDateString()}, {Email}";
    }
}