using Npgsql;
using v2.utils;

namespace v2.Db;

public class DatabaseSeeder : DatabaseAbstract
{
    public async Task SeedDbAsync()
    {
        Console.WriteLine("Seeding database...");

        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                await conn.OpenAsync();

                // Create BlogPost table
                await CreateTableAsync(Commands.CreateBlogPostTable, conn);
                await InsertDataIfBlogpostTableEmpty(conn);

                // Create Image Table
                await CreateTableAsync(Commands.CreateImageTable, conn);
                await InsertDataIfImageTableEmpty(conn);

                // Create User Table
                await CreateTableAsync(Commands.CreateUsersTable, conn);
                await InsertDataIfUsersTableEmpty(conn);


                Console.WriteLine("Seeded database successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while seeding the database: {ex}");
                await conn.CloseAsync();
                throw;
            }
        }
    }

    private async Task CreateTableAsync(string sqlCommand, NpgsqlConnection conn)
    {
        using var cmd = new NpgsqlCommand(sqlCommand, conn);

        await cmd.ExecuteNonQueryAsync();
    }

    private async Task InsertDataIfUsersTableEmpty(NpgsqlConnection conn)
    {
        using var cmd = new NpgsqlCommand(Commands.InsertIntoUsersIfEmpty, conn);

        // Set parameter values
        var hasher = new PasswordHasher();
        var saltedHashedPassword = hasher.RunPasswordHasher("password");


        cmd.Parameters.AddWithValue(":username", "test");
        cmd.Parameters.AddWithValue(":password", saltedHashedPassword);

        await cmd.ExecuteNonQueryAsync();
    }

    private async Task InsertDataIfBlogpostTableEmpty(NpgsqlConnection conn)
    {
        using var cmd = new NpgsqlCommand(Commands.InsertIntoBlogPostsIfEmpty, conn);

        // Set parameter values
        var now = DateTime.Now;

        cmd.Parameters.AddWithValue(":author", "The Dev");
        cmd.Parameters.AddWithValue(":title", "Cats: So many!");
        cmd.Parameters.AddWithValue(":content",
            "This is an example blog post to tell you that my cat Bennet is one cool cat. A tuxedo cat to be specific! ^w^");
        cmd.Parameters.AddWithValue(":timestamp", now);
        cmd.Parameters.AddWithValue(":likes", 3);

        await cmd.ExecuteNonQueryAsync();
    }

    private async Task InsertDataIfImageTableEmpty(NpgsqlConnection conn)
    {
        using var cmd = new NpgsqlCommand(Commands.InsertIntoImageTableIfEmpty, conn);

        // get image file 
        var currentDirectory = Directory.GetCurrentDirectory();
        var fullPath = Path.Combine(currentDirectory, "db/files/bennet-test.jpg");
        var byteArray = File.ReadAllBytes(fullPath);

        cmd.Parameters.AddWithValue(":blogpostId", 1);
        cmd.Parameters.AddWithValue(":name", "bennet-test-1");
        cmd.Parameters.AddWithValue(":img",
            byteArray); // SELECT encode(img::bytea, 'base64') AS image_content FROM images WHERE id = 1;


        await cmd.ExecuteNonQueryAsync();
    }
}