namespace StudentsCRM.Models;

public class Course
{
    // Свойства
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Ключи
    public List<Student> Students { get; set; } = new List<Student>();
}