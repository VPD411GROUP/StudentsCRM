using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentsCRM.Data;
using StudentsCRM.Interfaces;
using StudentsCRM.Models;
using StudentsCRM.Repository;
using System.Threading.Tasks;

class Program
{
    private static IStudentsRepository _repo = null!;
    static async Task Main(string[] args)
    {
        // Инициализируем БД и репозиторий
        var db = new StudentsDbContext();

        await db.Database.EnsureCreatedAsync();

        _repo = (IStudentsRepository) new StudentsRepository(db);

        Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ СТУДЕНТАМИ ===");
        var exit = false;
        while (!exit)
        {
            ShowMainMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ShowAllStudentsAsync();
                    break;

                case "2":
                    await AddNewStudentAsync();
                    break;

                case "3":
                    await UpdateStudentAsync();
                    break;

                case "4":
                    await DeleteStudentAsync();
                    break;

                case "5":
                    await ShowAllCoursesAsync();
                    break;

                case "6":
                    await AddNewCourseAsync();
                    break;

                case "7":
                    await EnrollStudentInCourseAsync();
                    break;

                case "8":
                    await ShowStudentsWithDetailsAsync();
                    break;

                case "9":
                    await ShowCoursesWithStudentsAsync();
                    break;

                case "10":
                    await AddGradeAsync();
                    break;

                case "0":
                    exit = true;
                    Console.WriteLine("\nДо свидания!");
                    break;

                default:
                    Console.WriteLine("\nНеверный выбор! Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ShowMainMenu()
    {
        Console.WriteLine("//////");
        Console.WriteLine(" === ГЛАВНОЕ МЕНЮ === ");
        Console.WriteLine("1. Показать всех студентов");
        Console.WriteLine("2. Добавить нового студента");
        Console.WriteLine("3. Обновить данные студента");
        Console.WriteLine("4. Удалить студента");
        Console.WriteLine("5. Показать все курсы");
        Console.WriteLine("6. Добавить новый курс");
        Console.WriteLine("7. Записать студента на курс");
        Console.WriteLine("8. Показать студентов с деталями");
        Console.WriteLine("9. Показать курсы со студентами");
        Console.WriteLine("10. Добавить оценку студенту");
        Console.WriteLine("0. Выход");
        Console.WriteLine("Выберите пункт меню: ");
    }

    static async Task ShowAllStudentsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== ВСЕ СТУДЕНТЫ ===\n");

        var students = await _repo.GetAllStudentsAsync();

        if(students.IsNullOrEmpty())
        {
            Console.WriteLine("Студенты не найдены");
            return;
        }

        foreach (var s in students)
        {
            Console.WriteLine($"ID: {s.Id}");
            Console.WriteLine($"Имя: {s.FirstName} {s.LastName}");
            Console.WriteLine($"Email: {s.Email}");
            Console.WriteLine($"Дата рождения: {s.BirthDate:dd.MM.yyyy}");
            Console.WriteLine(" --- ");
        }
    }

    static async Task AddNewStudentAsync()
    {
        Console.Clear();
        Console.WriteLine("== ДОБАВЛЕНИЕ НОВОГО СТУДЕНТА\n");

        try
        {
            Console.Write("Имя: ");
            var firstName = Console.ReadLine() ?? "";

            Console.Write("Фамилия: ");
            var lastName = Console.ReadLine() ?? "";

            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";

            Console.Write("Дата рождения: ");
            if (!DateTime.TryParse(Console.ReadLine(), out var birthDate))
            {
                Console.WriteLine("ОШИБКА: Неправильный формат даты!");
                return;
            }

            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                BirthDate = birthDate
            };

            await _repo.AddStudentAsync(student);
            Console.WriteLine("\n Студент успешно добавлен!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении студента: {ex.Message}");
        }
    }

    static async Task UpdateStudentAsync()
    {
        Console.Clear();
        Console.WriteLine("=== ОБНОВЛЕНИЕ СТУДЕНТА ===\n");

        await ShowAllStudentsAsync();
        Console.Write("\nВведите ID студента для обновления: ");

        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("ОШИБКА: Неправильный формат ID!");
            return;
        }

        var student = await _repo.GetStudentByIdAsync(studentId);
        if (student is null)
        {
            Console.WriteLine("ПРЕДУПРЕЖДЕНИЕ: Студент не найден!");
            return;
        }

        try
        {
            Console.WriteLine($"\nТекущие данные: ");
            Console.WriteLine($"Имя: {student.FirstName} {student.LastName}");
            Console.WriteLine($"Email: {student.Email}");
            Console.WriteLine($"Дата рождения: {student.BirthDate:dd.MM.yyyy}");

            Console.Write("\nНовое имя (оставьте пустым, чтобы не менять): ");
            var newFirstName = Console.ReadLine();
            if (!string.IsNullOrEmpty(newFirstName))
                student.FirstName = newFirstName;

            Console.Write("\nНовая фамилия: ");
            var newLastName = Console.ReadLine();
            if (!string.IsNullOrEmpty(newLastName))
                student.LastName = newLastName;

            Console.Write("\nНовый Email: ");
            var newEmail = Console.ReadLine();
            if (!string.IsNullOrEmpty(newEmail))
                student.Email = newEmail;

            await _repo.UpdateStudentAsync(student);
            Console.WriteLine("\nДанные студента обновлены!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при обновлении студента: " + ex.Message);
        }
    }

        static async Task DeleteStudentAsync()
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ СТУДЕНТА ===\n");

            await ShowAllStudentsAsync();

            Console.WriteLine("\n Введите ID студента для удаления: ");

            if (!int.TryParse(Console.ReadLine(), out var studentId))
            {
                Console.WriteLine("ОШИБКА: Неправильный формат ID!");
                return;
            }

            var student = await _repo.GetStudentByIdAsync(studentId);
            if (student is null)
            {
                Console.WriteLine("ПРЕДУПРЕЖДЕНИЕ: Студент не найден!");
                return;
            }

            Console.WriteLine($"\n Вы действительно хотите удалить этого студента " +
                $"[{studentId}: {student.FirstName} {student.LastName}]? (y/n)");

            var input = Console.ReadLine()?.ToLower();

            try
            {
                if (input == "yes" || input == "y" || input == "да" || input == "д")
                {
                    await _repo.DeleteStudentByIdAsync(studentId);
                    Console.WriteLine("Студент успешно удален!");
                }
                else
                {
                    Console.WriteLine("Удаление отменено");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении студента: " + ex.Message);
            }
        }

        static async Task ShowAllCoursesAsync()
        {
            Console.Clear();
            Console.WriteLine("=== ВСЕ КУРСЫ ===\n");

            var courses = await _repo.GetAllCoursesAsync();

            if (courses.IsNullOrEmpty())
            {
                Console.WriteLine("Курсы не найдены.");
                return;
            }

            foreach (var c in courses)
            {
                Console.WriteLine($"ID: {c.Id}");
                Console.WriteLine($"Название: {c.Name}");
                Console.WriteLine($"Описание: {c.Description}");
                Console.WriteLine(" --- ");
            }
        }

        static async Task AddNewCourseAsync()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ НОВОГО КУРСА ===\n");

            try
            {
                Console.Write("Название: ");
                var name = Console.ReadLine() ?? "";

                Console.Write("Описание: ");
                var description = Console.ReadLine() ?? "";

                var course = new Course
                {
                    Name = name,
                    Description = description
                };

                await _repo.AddCourseAsync(course);
                Console.WriteLine("\nКурс успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении курса: " + ex.Message);
            }
        }

        static async Task EnrollStudentInCourseAsync()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАПИСЬ СТУДЕНТА НА КУРС ===\n");

            await ShowAllStudentsAsync();

            Console.Write("\nВведите ID студента: ");

            if(!int.TryParse(Console.ReadLine(), out var studentId))
            {
                Console.WriteLine("ОШИБКА: неверный формат ID студента!");
                return;
            }

            await ShowAllCoursesAsync();
            Console.Write("\nВведите ID курса: ");

            if (!int.TryParse(Console.ReadLine(), out var courseId))
            {
                Console.WriteLine("ОШИБКА: Неверный формат ID курса!");
                return;
            }

            try
            {
                await _repo.EnrollStudentInCourseAsync(studentId, courseId);
                Console.WriteLine("\nСтудент успешно записан на курс!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при записи на курс: " + ex.Message);
            }
        }

        static async Task ShowStudentsWithDetailsAsync()
        {
            Console.Clear();
            Console.WriteLine("=== СТУДЕНТЫ С ДЕТАЛЯМИ ===\n");

            var students = await _repo.GetStudentsWithDetailsAsync();

            if(students.IsNullOrEmpty())
            {
                Console.WriteLine("Студенты не найдены");
                return;
            }

            foreach(var s in students)
            {
                Console.WriteLine($"ID: {s.Id}");
                Console.WriteLine($"Имя: {s.FirstName} {s.LastName}");
                Console.WriteLine($"Email: {s.Email}");
                Console.WriteLine("Курсы: ");
                if(!s.Courses.IsNullOrEmpty())
                {
                    foreach (var c in s.Courses)
                    {
                        Console.WriteLine($"   - {c.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("нет курсов");
                }

                Console.WriteLine("Оценки: ");
                if(!s.Grades.IsNullOrEmpty())
                {
                    foreach(var g in s.Grades)
                    {
                        Console.WriteLine($"   - {g.SubjectName}: {g.Score}");
                    }
                }
                else
                {
                    Console.WriteLine("оценок нет");
                }
                Console.WriteLine("---");
            }
        }

    static async Task ShowCoursesWithStudentsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== КУРСЫ СО СТУДЕНТАМИ ===");

        var courses = await _repo.GetCoursesWithStudentsAsync();
        if(!courses.IsNullOrEmpty())
        {
            Console.WriteLine("Курсы не найдены");
            return;
        }

        foreach (var c in courses)
        {
            Console.WriteLine($"Курс: {c.Name}");
            Console.WriteLine($"Описание: {c.Description}");
            Console.WriteLine("Студенты: ");

            if(!c.Students.IsNullOrEmpty())
            {
                foreach(var s in c.Students)
                {
                    Console.WriteLine($"    - {s.FirstName} {s.LastName} - {s.Email}");
                }
            }
            else
            {
                Console.WriteLine("нет студентов");
            }
            Console.WriteLine("---");
        }
    }

    static async Task AddGradeAsync()
    {
        Console.Clear();
        Console.WriteLine("=== ДОБАВЛЕНИЕ ОЦЕНКИ ===\n");

        await ShowAllStudentsAsync();

        Console.Write("\nВведите ID студента: ");

        if(!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("ОШИБКА: Неверный формат ID!");
            return;
        }

        var student = await _repo.GetStudentByIdAsync(studentId);

        if (student is null)
        {
            Console.WriteLine("ПРЕДУПРЕЖДЕНИЕ: Студент не найден!");
            return;
        }

        try
        {
            Console.Write("Предмет: ");
            var subject = Console.ReadLine() ?? "";

            Console.Write("Оценка(1-5): ");
            if(!int.TryParse(Console.ReadLine(), out var score) || score < 1 || score > 5)
            {
                Console.WriteLine("ОШИБКА: Ошибка ввода!");
                return;
            }

            var grade = new Grade
            {
                StudentId = studentId,
                SubjectName = subject,
                Score = score
            };

            await _repo.AddGradeAsync(grade);
            Console.WriteLine("\nОценка успешно добавлена!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при добавлении оценки: " + ex.Message);
        }
    }
}