using Npgsql;

namespace Db;

public class DatabaseSeeder() : DatabaseAbstract
{
    public async Task SeedDbAsync() {
        Console.WriteLine("Seeding database...");

        GetConnection();

        if (_connection is null) {
            throw new Exception("Connection is null");
        }

        try {
            await _connection.OpenAsync();

            await CreateBlogPostTableAsync();
            await InsertDataIfBlogpostTableEmpty();
            
            Console.WriteLine("Seeded database successfully");
        } catch (Exception Ex) {
            Console.WriteLine($"An error occured while seeding the database: {Ex}");
            throw;
        } finally {
            await _connection.DisposeAsync();
        }
    }

    private async Task CreateBlogPostTableAsync() {
         using var cmd = new NpgsqlCommand(
        @"CREATE TABLE IF NOT EXISTS blogposts (
            Id SERIAL PRIMARY KEY,
            Author VARCHAR(255) DEFAULT 'unassigned author',
            Title VARCHAR(255) DEFAULT 'unassigned title',
            Content TEXT,
            TimeStamp TIMESTAMP WITHOUT TIME ZONE,
            Likes INTEGER DEFAULT 0
        )",
        _connection);

    await cmd.ExecuteNonQueryAsync();
    }

    private async Task InsertDataIfBlogpostTableEmpty() {
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
        cmd.Parameters.AddWithValue(":content", "There are so many cats where I live. Loads of them!");
        cmd.Parameters.AddWithValue(":timestamp", now);
        cmd.Parameters.AddWithValue(":likes", 3);
 
        await cmd.ExecuteNonQueryAsync();
    }
}

