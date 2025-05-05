using System.Data;
using Npgsql;
using v2.Models;

namespace v2.Db;

public class ServerStorageImageDatabaseService : DatabaseAbstract
{
    public async Task<ServerStorageImage?> GetImageFile(int id)
    {
        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();

                using var cmd = new NpgsqlCommand(Commands.SelectServerStorageImage, conn);

                cmd.Parameters.AddWithValue(":blogpost_id", id);

                using var reader = await cmd.ExecuteReaderAsync();

                // handle 404
                if (await reader.ReadAsync() is false) return null;

                var imageRow = new ServerStorageImage(
                    reader.GetInt32(0), // Id
                    reader.GetInt32(1), // blogpost_id
                    reader.GetString(2), // name
                    reader.GetString(3), //alt
                    reader.GetString(4) //path
                );

                return imageRow;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured reading image: {ex}");
                await conn.CloseAsync();
                return null;
            }
        }
    }

    public async Task<bool> InsertImage(PostServerStorageImageRequest req)
    {
        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                await conn.OpenAsync();

                using var cmd = new NpgsqlCommand(Commands.InsertServerStorageImage, conn);

                cmd.Parameters.AddWithValue(":blogpostId", req.BlogpostId);
                cmd.Parameters.AddWithValue(":name", req.Name);
                cmd.Parameters.AddWithValue(":alt", req.Alt);
                cmd.Parameters.AddWithValue(":path", req.Path);

                var result = cmd.ExecuteScalar();
                result = result == DBNull.Value ? null : result;
                var id = Convert.ToInt32(result);

                await conn.CloseAsync();

                return id > 0;
            }
            catch (Exception ex)
            {
                // todo: add 400 error handling here
                Console.WriteLine($"An error occured creating the image: {ex}");
                await conn.CloseAsync();
                return false;
            }
        }
    }
}