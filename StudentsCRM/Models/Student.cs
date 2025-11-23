namespace StudentsCRM.Models;

public class Student
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; } 
    public DateTime BirthDate { get; set; }
    public required string Email { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {FirstName} {LastName}, {BirthDate.ToShortDateString()}, {Email}";
    }
}