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

            await CreateTableAsync(_commands.CreateBlogPostTable);
            await InsertDataIfBlogpostTableEmpty();

            await CreateTableAsync(_commands.CreateImageTable);

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
        using var cmd = new NpgsqlCommand(SqlCommand,_connection);

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
}

