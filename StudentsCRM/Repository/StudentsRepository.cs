using Microsoft.Data.SqlClient;
using StudentsCRM.Interfaces;
using StudentsCRM.Models;

namespace StudentsCRM.Repository;

// CRUD
// Create - Создать
// Read - Читать
// Update - Обновить
// Delete - Удалить

public class StudentsRepository(string connectionString) : IRepository
{
    private readonly string _connectionString = connectionString;

    // Метод для получения всех студентов
    public List<Student> GetAll()
    {
        // Создаем новый лист (копию)
        var students = new List<Student>();

        // Открываем соединение
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        // Задаем запрос
        var query = "SELECT * FROM Students";

        // Создаем команду
        using var command = new SqlCommand(query, connection);

        // Запускаем "читалку"
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var student = new Student
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                BirthDate = reader.GetDateTime(3),
                Email = reader.GetString(4),
            };

            // Добавляем нового студента
            students.Add(student);
        }

        // Возвращаем лист
        return students;
    }

    public Student? GetById(int id)
    {
        // Открываем соединение
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        // Пишем запрос
        var query = "SELECT Id, FirstName, LastName, BirthDate, Email FROM Students WHERE Id = @id";

        // Задаем команду
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id); // Задаем параметр

        // Читаем данные
        using var reader = command.ExecuteReader();

        // Если "читалка" запустилась, возвращаем нового студента
        if (reader.Read())
        {
            return new Student()
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                BirthDate = reader.GetDateTime(3),
                Email = reader.GetString(4),
            };
        }

        // Если студент не найден, возвращаем null
        return null;
    }

    public void Add(string firstName, string lastName, DateTime birthDate, string email)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "INSERT INTO Students (FirstName, LastName, BirthDate, Email) VALUES" +
            "(@firstName, @lastName, @birthDate, @email)";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@firstName", firstName);
        command.Parameters.AddWithValue("@lastName", lastName);
        command.Parameters.AddWithValue("@birthDate", birthDate);
        command.Parameters.AddWithValue("@email", email);

        command.ExecuteNonQuery();
    }

    public void Update(int id, string firstName, string lastName, DateTime birthDate, string email)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "UPDATE Students SET FirstName = @firstName, LastName = @lastName, BirthDate = @birthDate, Email = @email " +
            "WHERE Id = @id";

        using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@firstName", firstName);
        command.Parameters.AddWithValue("@lastName", lastName);
        command.Parameters.AddWithValue("@BirthDate", birthDate);
        command.Parameters.AddWithValue("@email", email);

        command.ExecuteNonQuery();

    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM Students WHERE Id = @id";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }
}
