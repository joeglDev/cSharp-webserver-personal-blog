using Npgsql;

namespace Db;

public class DatabaseSeeder() : DatabaseAbstract
{
    public async Task SeedDbAsync()
    {
        Console.WriteLine("Seeding database...");

        GetConnection();

        if (_connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {
            await _connection.OpenAsync();

            // Create BlogPost table
            await CreateTableAsync(_commands.CreateBlogPostTable);
            await InsertDataIfBlogpostTableEmpty();

            // Create Image Table
            await CreateTableAsync(_commands.CreateImageTable);
            await InsertDataIfImageTableEmpty();

            Console.WriteLine("Seeded database successfully");
        }
        catch (Exception Ex)
        {
            Console.WriteLine($"An error occured while seeding the database: {Ex}");
            throw;
        }
        finally
        {
            await _connection.DisposeAsync();
        }
    }

    private async Task CreateTableAsync(string SqlCommand)
    {
        using var cmd = new NpgsqlCommand(SqlCommand, _connection);

        await cmd.ExecuteNonQueryAsync();
    }

    private async Task InsertDataIfBlogpostTableEmpty()
    {
        string insertQuery = @"INSERT INTO blogposts (Author, Title, Content, TimeStamp, Likes)
SELECT :author, :title, :content, :timestamp, :likes
WHERE NOT EXISTS (
    SELECT 1 FROM blogposts WHERE Author = :author AND Title = :title
);";

        using var cmd = new NpgsqlCommand(insertQuery, _connection);

        // Set parameter values
        DateTime now = DateTime.Now;

        cmd.Parameters.AddWithValue(":author", "The Dev");
        cmd.Parameters.AddWithValue(":title", "Cats: So many!");
        cmd.Parameters.AddWithValue(":content", "This is an example blog post to tell you that my cat Bennet is one cool cat. A tuxedo cat to be specific! ^w^");
        cmd.Parameters.AddWithValue(":timestamp", now);
        cmd.Parameters.AddWithValue(":likes", 3);

        await cmd.ExecuteNonQueryAsync();
    }

    private async Task InsertDataIfImageTableEmpty()
    {
        string insertQuery = @"
INSERT INTO images (blogpost_id, name, img)
SELECT :blogpostId, :name, :img
WHERE NOT EXISTS (
    SELECT 1 FROM images WHERE blogpost_id = :blogpostId
)";
        using var cmd = new NpgsqlCommand(insertQuery, _connection);

        // get image file 
        var currentDirectory = Directory.GetCurrentDirectory();
        var fullPath = Path.Combine(currentDirectory, "db/files/bennet-test.jpg");
        byte[] byteArray = File.ReadAllBytes(fullPath);

        cmd.Parameters.AddWithValue(":blogpostId", 1);
        cmd.Parameters.AddWithValue(":name", "bennet-test-1");
        cmd.Parameters.AddWithValue(":img", byteArray); // SELECT encode(img::bytea, 'base64') AS image_content FROM images WHERE id = 1;


        await cmd.ExecuteNonQueryAsync();
    }
}

