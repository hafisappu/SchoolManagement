using SchoolManage.Data;
using System.Data;
using System.Data.SqlClient;

public interface IStandardService
{
    Task<List<Standard>> GetStandards();
    Task AddStandard(Standard standard);
    Task UpdateStandard(Standard standard);
    Task DeleteStandard(int standardId);
}

public class StandardService : IStandardService
{
    private readonly string _connectionString;

    public StandardService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Schooldb");
    }

    public async Task<List<Standard>> GetStandards()
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("GetStandards", connection); // Name of the stored procedure
        command.CommandType = CommandType.StoredProcedure;

        using var reader = await command.ExecuteReaderAsync();

        var standards = new List<Standard>();
        while (await reader.ReadAsync())
        {
            standards.Add(new Standard
            {
                StandardId = reader.GetInt32(reader.GetOrdinal("StandardId")),
                StandardName = reader.GetString(reader.GetOrdinal("StandardName"))
            });
        }

        return standards;
    }


    public async Task AddStandard(Standard standard)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("AddStandard", connection); // Name of the stored procedure
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@StandardName", standard.StandardName);

        await command.ExecuteNonQueryAsync();
    }


    public async Task UpdateStandard(Standard standard)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("UpdateStandard", connection); // Name of the stored procedure
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@StandardId", standard.StandardId);
        command.Parameters.AddWithValue("@StandardName", standard.StandardName);

        await command.ExecuteNonQueryAsync();
    }


    public async Task DeleteStandard(int standardId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("DeleteStandard", connection); // Name of the stored procedure
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@StandardId", standardId);

        await command.ExecuteNonQueryAsync();
    }


}
