using System.Data;
using Npgsql;
using v2.Models;

namespace v2.Db;

public class ImageDatabaseService : DatabaseAbstract
{
    private static async Task<byte[]> ReadBytesFromReader(NpgsqlDataReader reader, int ordinal)
    {
        const int bufferSize = 4096;
        var buffer = new byte[bufferSize];
        long bytesRead;

        using (var ms = new MemoryStream())
        {
            while ((bytesRead = reader.GetBytes(ordinal, ms.Position, buffer, 0, bufferSize)) > 0)
                await ms.WriteAsync(buffer, 0, (int)bytesRead);

            return ms.ToArray();
        }
    }


    public async Task<List<ImageRow>> GetAllImages()
    {
        List<ImageRow> images = [];

        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                await conn.OpenAsync();

                using var cmd = new NpgsqlBatch(conn)
                {
                    BatchCommands =
                    {
                        new NpgsqlBatchCommand(Commands.SelectAllImages)
                    }
                };

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    images.Add(new ImageRow(
                        reader.GetInt32(0), // Id
                        reader.GetInt32(1), // blogpost_id
                        reader.GetString(2), // name
                        await ReadBytesFromReader(reader, 3)
                    ));

                await reader.CloseAsync();
                return images;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured reading all images: {ex}");
                return [];
            }
        }
    }

    public async Task<ImageRow?> GetImageById(int blogpostId)
    {
        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();

                using var cmd = new NpgsqlCommand(Commands.SelectImage, conn);

                cmd.Parameters.AddWithValue(":blogpost_id", blogpostId);

                using var reader = await cmd.ExecuteReaderAsync();

                // handle 404
                if (await reader.ReadAsync() is false) return null;

                var image = new ImageRow(
                    reader.GetInt32(0), // Id
                    reader.GetInt32(1), // blogpost_id
                    reader.GetString(2), // name
                    await ReadBytesFromReader(reader, 3)
                );

                return image;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured reading image: {ex}");
                await conn.CloseAsync();
                return null;
            }
        }
    }

    public async Task<bool> InsertImage(ImageRow newImage)
    {
        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                await conn.OpenAsync();

                using var cmd = new NpgsqlCommand(Commands.InsertImage, conn);

                cmd.Parameters.AddWithValue(":blogpostId", newImage.BlogpostId);
                cmd.Parameters.AddWithValue(":name", newImage.Name);
                cmd.Parameters.AddWithValue(":img", newImage.Img);

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

    public async Task<bool?> DeleteImage(int blogpostId)
    {
        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                await conn.OpenAsync();

                using var cmd = new NpgsqlCommand(Commands.DeleteImage, conn);

                cmd.Parameters.AddWithValue(":blogpost_id", blogpostId);

                var result = await cmd.ExecuteNonQueryAsync();

                await conn.CloseAsync();

                return result > 0 ? true : null;
            }
            catch (Exception ex)
            {
                // todo: add 400 error handling
                Console.WriteLine($"An error occured deleting image: {ex}");
                await conn.CloseAsync();
                return false;
            }
        }
    }
}