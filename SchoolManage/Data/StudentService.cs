using System.Data;
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

            using var command = new SqlCommand("GetStudentsWithStandards", connection); // Name of the stored procedure
            command.CommandType = CommandType.StoredProcedure;

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
            
            using var command = new SqlCommand("AddStudent", connection); // Name of the stored procedure
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@StudentName", student.StudentName);
            command.Parameters.AddWithValue("@Age", student.Age);
            command.Parameters.AddWithValue("@StandardId", student.StandardId);

            await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateStudent(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("UpdateStudent", connection); // Name of the stored procedure
            command.CommandType = CommandType.StoredProcedure;

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

            using var command = new SqlCommand("DeleteStudent", connection); // Name of the stored procedure
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@StudentId", studentId);

            await command.ExecuteNonQueryAsync();
        }


        public async Task<List<Student>> GetStudentsByStandard(int standardId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("GetStudentsByStandard", connection); // Name of the stored procedure
            command.CommandType = CommandType.StoredProcedure;

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