using Npgsql;
using Webserver.Controllers;

namespace Db;

using Webserver.Models;

public class DatabaseService() : DatabaseAbstract
{
   public async Task<List<BlogPost>> GetAllBlogPosts() {
        List<BlogPost> posts = [];

        GetConnection();

        if (_connection is null) {
            throw new Exception("Connection is null");
        }

    try {

        await _connection.OpenAsync();

            using var cmd = new NpgsqlBatch(_connection) 
            {
                BatchCommands = {
                    new NpgsqlBatchCommand("SELECT * FROM blogposts;")
                }
            };

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) 
            {
                posts.Add(new BlogPost(
        reader.GetInt32(ordinal: 0), // Id
        reader.GetString(ordinal: 1), // Author
        reader.GetString(ordinal: 2), // Title
        reader.GetString(ordinal: 3), // Content
        reader.GetDateTime(ordinal: 4), // TimeStamp
        reader.GetInt32(ordinal: 5)   // Likes
    ));
            }

            await reader.CloseAsync();
            return posts;

    } catch (Exception Ex) {
        Console.WriteLine($"An error occured reading all blog posts: {Ex}");
        return [];
    } finally {
        await _connection.DisposeAsync();
    }
   }
 
}
