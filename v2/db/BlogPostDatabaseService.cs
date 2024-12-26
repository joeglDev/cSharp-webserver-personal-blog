using Npgsql;
using v2.Models;

namespace v2.Db;

public class BlogPostDatabaseService : DatabaseAbstract
{
    public async Task<List<BlogPost>> GetAllBlogPosts()
    {
        List<BlogPost> posts = [];

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
                    new NpgsqlBatchCommand(Commands.SelectAllBlogPosts)
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

    public async Task<bool> InsertBlogPost(BlogPost newPost)
    {

        GetConnection();

        if (Connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {

            await Connection.OpenAsync();

            using var cmd = new NpgsqlCommand(Commands.InsertBlogPost, Connection);

            cmd.Parameters.AddWithValue(":Author", newPost.Author);
            cmd.Parameters.AddWithValue(":Title", newPost.Title);
            cmd.Parameters.AddWithValue(":Content", newPost.Content);
            cmd.Parameters.AddWithValue(":TimeStamp", newPost.TimeStamp);
            cmd.Parameters.AddWithValue(":Likes", newPost.Likes);

            object? result = cmd.ExecuteScalar();
            result = (result == DBNull.Value) ? null : result;
            int id = Convert.ToInt32(result);

            return id > 0;

        }
        catch (Exception ex)
        {
            // todo: add 400 vs 500 error handling here
            Console.WriteLine($"An error occured creating a blog post: {ex}");
            await Connection.DisposeAsync();
            return false;
        }
        finally
        {
            await Connection.DisposeAsync();
        }
    }

    public async Task<bool> DeleteBlogPost(int id)
    {

        GetConnection();

        if (Connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {

            await Connection.OpenAsync();

            using var cmd = new NpgsqlCommand(Commands.DeleteBlogPost, Connection);

            cmd.Parameters.AddWithValue(":Id", id);

            int affectedRows = await cmd.ExecuteNonQueryAsync();
            return affectedRows > 0;

        }
        catch (Exception ex)
        {
            // todo: add 400 vs 500 error handling here
            Console.WriteLine($"An error occured deleting a blog post: {ex}");
            await Connection.DisposeAsync();
            return false;
        }
        finally
        {
            await Connection.DisposeAsync();
        }
    }

    public async Task<BlogPost?> PatchBlogPost(int id, BlogPost updatedPost)
    {

        GetConnection();

        if (Connection is null)
        {
            throw new Exception("Connection is null");
        }

        try
        {

            await Connection.OpenAsync();

            using var cmd = new NpgsqlCommand(Commands.UpdateBlogPost, Connection);

            cmd.Parameters.AddWithValue(":Author", updatedPost.Author);
            cmd.Parameters.AddWithValue(":Title", updatedPost.Title);
            cmd.Parameters.AddWithValue(":Content", updatedPost.Content);
            cmd.Parameters.AddWithValue(":TimeStamp", updatedPost.TimeStamp);
            cmd.Parameters.AddWithValue(":Likes", updatedPost.Likes);
            cmd.Parameters.AddWithValue(":Id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                new BlogPost(
                    reader.GetInt32(ordinal: 0), // Id
                    reader.GetString(ordinal: 1), // Author
                    reader.GetString(ordinal: 2), // Title
                    reader.GetString(ordinal: 3), // Content
                    reader.GetDateTime(ordinal: 4), // TimeStamp
                    reader.GetInt32(ordinal: 5)   // Likes
                );
            }

            await reader.CloseAsync();

            return updatedPost;
        }
        catch (Exception ex)
        {
            // todo: add 400 vs 500 error handling here
            Console.WriteLine($"An error occured patching a blog post: {ex}");
            await Connection.DisposeAsync();
            return null;
        }
        finally
        {
            await Connection.DisposeAsync();
        }
    }
}
