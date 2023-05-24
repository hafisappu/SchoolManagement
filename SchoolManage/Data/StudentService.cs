using System.Data.SqlClient;

namespace SchoolManage.Data
{
    public interface IStudentService
    {
        Task<List<Student>> GetStudents();
        Task AddStudent(Student student);
        Task UpdateStudent(Student student);
        Task DeleteStudent(int studentId);
        Task<List<Student>> GetStudentsByStandard(int standardId);
    }

    public class StudentService : IStudentService
    {
        private readonly string _connectionString;

        public StudentService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Schooldb");
        }

        public async Task<List<Student>> GetStudents()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT * FROM Students INNER JOIN Standards ON Students.StandardId = Standards.StandardId", connection);
            using var reader = await command.ExecuteReaderAsync();

            var students = new List<Student>();
            while (await reader.ReadAsync())
            {
                students.Add(new Student
                {
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                    Age = reader.GetInt32(reader.GetOrdinal("Age")),
                    Standard = new Standard
                    {
                        StandardId = reader.GetInt32(reader.GetOrdinal("StandardId")),
                        StandardName = reader.GetString(reader.GetOrdinal("StandardName"))
                    }
                });
            }
            return students;
        }

        public async Task AddStudent(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("INSERT INTO Students (StudentName, Age, StandardId) VALUES (@StudentName, @Age, @StandardId)", connection);
            command.Parameters.AddWithValue("@StudentName", student.StudentName);
            command.Parameters.AddWithValue("@Age", student.Age);
            command.Parameters.AddWithValue("@StandardId", student.StandardId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateStudent(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("UPDATE Students SET StudentName = @StudentName, Age = @Age, StandardId = @StandardId WHERE StudentId = @StudentId", connection);
            command.Parameters.AddWithValue("@StudentId", student.StudentId);
            command.Parameters.AddWithValue("@StudentName", student.StudentName);
            command.Parameters.AddWithValue("@Age", student.Age);
            command.Parameters.AddWithValue("@StandardId", student.StandardId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteStudent(int studentId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("DELETE FROM Students WHERE StudentId = @StudentId", connection);
            command.Parameters.AddWithValue("@StudentId", studentId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Student>> GetStudentsByStandard(int standardId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT * FROM Students WHERE StandardId = @StandardId", connection);
            command.Parameters.AddWithValue("@StandardId", standardId);
            using var reader = await command.ExecuteReaderAsync();

            var students = new List<Student>();
            while (await reader.ReadAsync())
            {
                students.Add(new Student
                {
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                    Age = reader.GetInt32(reader.GetOrdinal("Age")),
                    StandardId = reader.GetInt32(reader.GetOrdinal("StandardId"))
                });
            }
            return students;
        }

    }
}