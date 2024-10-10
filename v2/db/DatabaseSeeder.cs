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
}

