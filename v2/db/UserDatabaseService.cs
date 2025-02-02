using System.Data;
using Npgsql;
using v2.Models;

namespace v2.Db;

public class UserDatabaseService : DatabaseAbstract
{

    public async Task<string?> GetPasswordByUsername(string username)
    {
        using (var conn = GetIndividualConnection())
        {
            if (conn is null)
            {
                throw new Exception("Connection is null");
            }

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }

                using var cmd = new NpgsqlCommand(Commands.SelectPasswordByUsername, conn);

                cmd.Parameters.AddWithValue(":username", username);

                using var reader = await cmd.ExecuteReaderAsync();

                // handle 404
                if (await reader.ReadAsync() is false)
                {
                    return null;
                }
                else
                {
                    var hashedPassword = reader["password"].ToString();
                    return hashedPassword;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured reading user password: {ex}");
                await conn.CloseAsync();
                return null;
            }
        }
    }
}