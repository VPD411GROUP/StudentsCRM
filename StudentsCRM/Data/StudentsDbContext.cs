using StudentsCRM.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentsCRM.Data;
public class StudentsDbContext : DbContext
{
    // Таблицы в БД 
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Grade> Grades { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;

    // Конфигурация БД
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=DESKTOP-18;Database=StudentsDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString);
    }

    // Настройка связей
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Многие-ко-многим между Student и Course
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Courses)
            .WithMany(s => s.Students)
            .UsingEntity(j => j.ToTable("StudentCourses"));

        // Один-ко-мнгим между Student и Grade
        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Student)
            .WithMany(s => s.Grades)
            .HasForeignKey(g => g.StudentId);
    }
}
