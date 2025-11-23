using StudentsCRM.Interfaces;
using StudentsCRM.Repository;

class Program
{
    static void Main(string[] args)
    {
        string _connectionString = "Server=DESKTOP-18;Database=StudentsDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True";

        var repository = new StudentsRepository(_connectionString);

        Console.WriteLine("=== CONSOLE APP STUDENTS CRM");
        Console.WriteLine();
        Console.WriteLine("1 - Получить всех студентов");
        Console.WriteLine("2 - Получить студента по ID");
        Console.WriteLine("3 - Добавить студента");
        Console.WriteLine("exit - Выход");

        while(true)
        {
            var input = Console.ReadLine()?.ToLower();
            switch (input)
            {
                case "1":
                    var students = repository.GetAll();
                    foreach (var s in students)
                    {
                        Console.WriteLine($"ID: {s.Id}, Имя: {s.FirstName}, Фамилия: {s.LastName}, " +
                            $"День рождения: {s.BirthDate.ToShortDateString()}, Email: {s.Email}");
                    }
                    break;

                case "2":
                    Console.WriteLine("Введите ID:");
                    Int32.TryParse(Console.ReadLine(), out var id);
                    var student = repository.GetById(id);
                    if (student == null)
                        Console.WriteLine("Студент не найден");

                    Console.WriteLine(student?.ToString());

                    break;

                case "3":
                        Console.WriteLine("=== Добавление ===");

                        Console.WriteLine("Имя: ");
                        var name = Console.ReadLine();

                        Console.WriteLine("Фамилия: ");
                        var surname = Console.ReadLine();

                        Console.WriteLine("Дата рождения: ");
                        var birthDate = DateTime.Parse(Console.ReadLine());

                        Console.WriteLine("Email: ");
                        var email = Console.ReadLine();

                        Console.WriteLine("Начинаем добавлять...");

                        repository.Add(name, surname, birthDate, email);

                    Console.WriteLine("Успешно завершено!");
                        break;

                case "4":
                    Console.WriteLine("=== Обновление ===");

                    Console.WriteLine("ID: ");
                    Int32.TryParse(Console.ReadLine(), out int updatingId);

                    Console.WriteLine("Имя: ");
                    var updatingName = Console.ReadLine();

                    Console.WriteLine("Фамилия: ");
                    var updatingSurname = Console.ReadLine();

                    Console.WriteLine("Дата рождения: ");
                    var updatingBirthDate = DateTime.Parse(Console.ReadLine());

                    Console.WriteLine("Email: ");
                    var updatingEmail = Console.ReadLine();

                    Console.WriteLine("Начинаем обновление...");

                    repository.Update(updatingId, updatingName, updatingSurname, updatingBirthDate, updatingEmail);

                    Console.WriteLine("Успешно завершено!");
                    break;

                case "5":
                    Console.WriteLine("=== Удаление ===");
                    Console.WriteLine("Id: ");

                    Int32.TryParse(Console.ReadLine(), out int deletingId);

                    Console.WriteLine("Удаляем...");

                    repository.Delete(deletingId);

                    Console.WriteLine("Успешно завершено!");
                    break;

                case "exit":
                    Environment.Exit(1);
                    break;

                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }
}