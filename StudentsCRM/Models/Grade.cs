namespace StudentsCRM.Models;

public class Grade
{
    // Свойства
    public int Id { get; set; }
    public required string SubjectName { get; set; }
    public int Score { get; set; }

    // Внешние ключи
    public int StudentId { get; set; }
    public required Student Student { get; set; }
}
