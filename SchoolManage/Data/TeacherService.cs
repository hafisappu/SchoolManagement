using System.Data;
using System.Data.SqlClient;

namespace SchoolManage.Data
{
    public interface ITeacherService
    {
        Task<List<Teacher>> GetTeachers();
        Task AddTeacher(Teacher teacher);
        Task UpdateTeacher(Teacher teacher);
        Task DeleteTeacher(int teacherId);
        Task<List<Teacher>> GetTeachersByStandard(int standardId);
    }

    public class TeacherService : ITeacherService
    {
        private readonly string _connectionString;

        public TeacherService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Schooldb");
        }

        public async Task<List<Teacher>> GetTeachers()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("GetTeachers", connection); // Name of the stored procedure
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();

            var teachers = new List<Teacher>();
            while (await reader.ReadAsync())
            {
                teachers.Add(new Teacher
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                    Standard = new Standard
                    {
                        StandardId = reader.GetInt32(reader.GetOrdinal("StandardIdNew")),
                        StandardName = reader.GetString(reader.GetOrdinal("StandardNameNew"))
                    }
                });
            }

            return teachers;
        }


        public async Task AddTeacher(Teacher teacher)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("AddTeacher", connection); // Name of the stored procedure
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Name", teacher.Name);
            command.Parameters.AddWithValue("@MobileNumber", teacher.MobileNumber);
            command.Parameters.AddWithValue("@StandardId", teacher.StandardId);

            await command.ExecuteNonQueryAsync();
        }


        public async Task UpdateTeacher(Teacher teacher)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("UpdateTeacher", connection); // Name of the stored procedure
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", teacher.Id);
            command.Parameters.AddWithValue("@Name", teacher.Name);
            command.Parameters.AddWithValue("@MobileNumber", teacher.MobileNumber);
            command.Parameters.AddWithValue("@StandardId", teacher.StandardId);

            await command.ExecuteNonQueryAsync();
        }


        public async Task DeleteTeacher(int teacherId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("DeleteTeacher", connection); // Name of the stored procedure
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", teacherId);

            await command.ExecuteNonQueryAsync();
        }


        public async Task<List<Teacher>> GetTeachersByStandard(int standardId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("GetTeachersByStandard", connection); // Name of the stored procedure
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@StandardId", standardId);

            using var reader = await command.ExecuteReaderAsync();

            var teachers = new List<Teacher>();
            while (await reader.ReadAsync())
            {
                teachers.Add(new Teacher
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                    StandardId = reader.GetInt32(reader.GetOrdinal("StandardId"))
                });
            }

            return teachers;
        }


    }

}
