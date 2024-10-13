using Npgsql;
using Webserver.Models;

namespace Db;

public class ImageDatabaseService() : DatabaseAbstract
{
    private static async Task<byte[]> ReadBytesFromReader(NpgsqlDataReader reader, int ordinal)
    {
        const int bufferSize = 4096;
        byte[] buffer = new byte[bufferSize];
        long bytesRead;

        using (var ms = new MemoryStream())
        {
            while ((bytesRead = reader.GetBytes(ordinal, ms.Position, buffer, 0, bufferSize)) > 0)
            {
                await ms.WriteAsync(buffer, 0, (int)bytesRead);
            }

            return ms.ToArray();
        }
    }


    public async Task<List<ImageRow>> GetAllImages()
    {
        List<ImageRow> images = [];

        GetConnection();

        if (_connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {

            await _connection.OpenAsync();

            using var cmd = new NpgsqlBatch(_connection)
            {
                BatchCommands = {
                    new NpgsqlBatchCommand("SELECT * FROM images;")
                }
            };

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                images.Add(new ImageRow(
                reader.GetInt32(ordinal: 0), // Id
                reader.GetInt32(ordinal: 1), // blogpost_id
                reader.GetString(ordinal: 2), // name
                await ReadBytesFromReader(reader, 3)

    ));
            }

            await reader.CloseAsync();
            return images;

        }
        catch (Exception Ex)
        {
            Console.WriteLine($"An error occured reading all blog posts: {Ex}");
            return [];
        }
        finally
        {
            await _connection.DisposeAsync();
        }
    }

    public async Task<ImageRow?> GetImageById(int Id)
    {
        List<ImageRow> images = [];

        GetConnection();

        if (_connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {
            List<ImageRow> Images = [];
            await _connection.OpenAsync();

            using var cmd = new NpgsqlCommand("SELECT * FROM images WHERE id = :id;", _connection);

            cmd.Parameters.AddWithValue(":id", Id);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Images.Add(new ImageRow(
                reader.GetInt32(ordinal: 0), // Id
                reader.GetInt32(ordinal: 1), // blogpost_id
                reader.GetString(ordinal: 2), // name
                await ReadBytesFromReader(reader, 3)

    ));
            }

            await reader.CloseAsync();
            return Images[0];
        }
        catch (Exception Ex)
        {
            Console.WriteLine($"An error occured reading all blog posts: {Ex}");
            return null;
        }
        finally
        {
            await _connection.DisposeAsync();
        }
    }

    public async Task<bool> InsertImage(ImageRow NewImage)
    {

        GetConnection();

        if (_connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {

            await _connection.OpenAsync();

            string sql = @"
INSERT INTO images (blogpost_id, name, img)
VALUES (:blogpostId, :name, :img) RETURNING ID";

            using var cmd = new NpgsqlCommand(sql, _connection);

            cmd.Parameters.AddWithValue(":blogpostId", NewImage.BlogpostId);
            cmd.Parameters.AddWithValue(":name", NewImage.Name);
            cmd.Parameters.AddWithValue(":img", NewImage.Img);

            object? result = cmd.ExecuteScalar();
            result = (result == DBNull.Value) ? null : result;
            int id = Convert.ToInt32(result);

            return id > 0;

        }
        catch (Exception Ex)
        {
            // todo: add 400 vs 500 error handling here
            Console.WriteLine($"An error occured creating the image: {Ex}");
            await _connection.DisposeAsync();
            return false;
        }
        finally
        {
            await _connection.DisposeAsync();
        }
    }

    public async Task<bool> DeleteImage(int Id)
    {

        GetConnection();

        if (_connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {

            await _connection.OpenAsync();

            string sql = "DELETE FROM images WHERE Id = :Id";

            using var cmd = new NpgsqlCommand(sql, _connection);

            cmd.Parameters.AddWithValue(":Id", Id);

            int affectedRows = await cmd.ExecuteNonQueryAsync();
            return affectedRows > 0;

        }
        catch (Exception Ex)
        {
            // todo: add 400 vs 500 error handling here
            Console.WriteLine($"An error occured deleting image: {Ex}");
            await _connection.DisposeAsync();
            return false;
        }
        finally
        {
            await _connection.DisposeAsync();
        }
    }
}