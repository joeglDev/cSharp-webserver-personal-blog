using Npgsql;
using Webserver.Models;

namespace Db;

public class DatabaseService() : DatabaseAbstract
{
    public async Task<List<BlogPost>> GetAllBlogPosts()
    {
        List<BlogPost> posts = [];

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

    public async Task<bool> InsertBlogPost(BlogPost NewPost)
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
            INSERT INTO BlogPosts (
                Author,
                Title,
                Content,
                TimeStamp,
                Likes
            )
            VALUES (
                :Author,
                :Title,
                :Content,
                :TimeStamp,
                :Likes
            )
            RETURNING Id";

            using var cmd = new NpgsqlCommand(sql, _connection);

            cmd.Parameters.AddWithValue(":Author", NewPost.Author);
            cmd.Parameters.AddWithValue(":Title", NewPost.Title);
            cmd.Parameters.AddWithValue(":Content", NewPost.Content);
            cmd.Parameters.AddWithValue(":TimeStamp", NewPost.TimeStamp);
            cmd.Parameters.AddWithValue(":Likes", NewPost.Likes);

            object? result = cmd.ExecuteScalar();
            result = (result == DBNull.Value) ? null : result;
            int id = Convert.ToInt32(result);

            return id > 0;

        }
        catch (Exception Ex)
        {
            // todo: add 400 vs 500 error handling here
            Console.WriteLine($"An error occured creating a blog post: {Ex}");
            await _connection.DisposeAsync();
            return false;
        }
        finally
        {
            await _connection.DisposeAsync();
        }
    }

       public async Task<bool> DeleteBlogPost(int Id)
    {

        GetConnection();

        if (_connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {

            await _connection.OpenAsync();

            string sql = @"DELETE FROM blogposts WHERE Id = :Id";

            using var cmd = new NpgsqlCommand(sql, _connection);

            cmd.Parameters.AddWithValue(":Id", Id);

            int affectedRows = await cmd.ExecuteNonQueryAsync();
            return affectedRows > 0;

        }
        catch (Exception Ex)
        {
            // todo: add 400 vs 500 error handling here
            Console.WriteLine($"An error occured deleting a blog post: {Ex}");
            await _connection.DisposeAsync();
            return false;
        }
        finally
        {
            await _connection.DisposeAsync();
        }
    }

}
