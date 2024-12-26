using Npgsql;

namespace v2.Db;

public class DatabaseSeeder() : DatabaseAbstract
{
    public async Task SeedDbAsync()
    {
        Console.WriteLine("Seeding database...");

        GetConnection();

        if (Connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {
            await Connection.OpenAsync();

            // Create BlogPost table
            await CreateTableAsync(Commands.CreateBlogPostTable);
            await InsertDataIfBlogpostTableEmpty();

            // Create Image Table
            await CreateTableAsync(Commands.CreateImageTable);
            await InsertDataIfImageTableEmpty();

            Console.WriteLine("Seeded database successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while seeding the database: {ex}");
            throw;
        }
        finally
        {
            await Connection.DisposeAsync();
        }
    }

    private async Task CreateTableAsync(string sqlCommand)
    {
        using var cmd = new NpgsqlCommand(sqlCommand, Connection);

        await cmd.ExecuteNonQueryAsync();
    }

    private async Task InsertDataIfBlogpostTableEmpty()
    {

        using var cmd = new NpgsqlCommand(Commands.InsertIntoBlogPostsIfEmpty, Connection);

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
        using var cmd = new NpgsqlCommand(Commands.InsertIntoImageTableIfEmpty, Connection);

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

