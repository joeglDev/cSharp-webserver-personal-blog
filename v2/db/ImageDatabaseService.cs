using Npgsql;
using v2.Models;

namespace v2.Db;

// TODO: move connection to creation of class
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

        if (Connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {

            await Connection.OpenAsync();

            using var cmd = new NpgsqlBatch(Connection)
            {
                BatchCommands = {
                    new NpgsqlBatchCommand(Commands.SelectAllImages)
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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured reading all blog posts: {ex}");
            return [];
        }
        finally
        {
            await Connection.DisposeAsync();
        }
    }
    
    // TODO: add http 404 not found err handling
    public async Task<ImageRow?> GetImageById(int blogpostId)
    {
        GetConnection();

        if (Connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {
            List<ImageRow> images = [];
            await Connection.OpenAsync(); // TODO: NOT ASYNC???

            using var cmd = new NpgsqlCommand(Commands.SelectImage, Connection);

            cmd.Parameters.AddWithValue(":blogpost_id", blogpostId);

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
            return images[0]; // FIX THIS
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured reading all blog posts: {ex}");
            return null;
        }
        finally
        {
            await Connection.DisposeAsync();
        }
    }

    public async Task<bool> InsertImage(ImageRow newImage)
    {

        GetConnection();

        if (Connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {

            await Connection.OpenAsync();

            using var cmd = new NpgsqlCommand(Commands.InsertImage, Connection);

            cmd.Parameters.AddWithValue(":blogpostId", newImage.BlogpostId);
            cmd.Parameters.AddWithValue(":name", newImage.Name);
            cmd.Parameters.AddWithValue(":img", newImage.Img);

            object? result = cmd.ExecuteScalar();
            result = (result == DBNull.Value) ? null : result;
            int id = Convert.ToInt32(result);

            return id > 0;

        }
        catch (Exception ex)
        {
            // todo: add 400 vs 500 error handling here
            Console.WriteLine($"An error occured creating the image: {ex}");
            await Connection.DisposeAsync();
            return false;
        }
        finally
        {
            await Connection.DisposeAsync();
        }
    }

    public async Task<bool> DeleteImage(int blogpostId)
    {

        GetConnection();

        if (Connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {

            await Connection.OpenAsync();

            using var cmd = new NpgsqlCommand(Commands.DeleteImage, Connection);

            cmd.Parameters.AddWithValue(":blogpost_id", blogpostId);

            int affectedRows = await cmd.ExecuteNonQueryAsync();
            return affectedRows > 0;

        }
        catch (Exception ex)
        {
            // todo: add 400 vs 500 error handling here
            Console.WriteLine($"An error occured deleting image: {ex}");
            await Connection.DisposeAsync();
            return false;
        }
        finally
        {
            await Connection.DisposeAsync();
        }
    }
}