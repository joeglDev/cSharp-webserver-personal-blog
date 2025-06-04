using Npgsql;
using v2.Models;

namespace v2.Db;

public class BlogPostDatabaseService : DatabaseAbstract
{
    public async Task<List<BlogPost>> GetAllBlogPosts()
    {
        List<BlogPost> posts = [];

        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                await conn.OpenAsync();

                using var cmd = new NpgsqlBatch(conn)
                {
                    BatchCommands =
                    {
                        new NpgsqlBatchCommand(Commands.SelectAllBlogPosts)
                    }
                };

                using var reader = await cmd.ExecuteReaderAsync();
                Console.WriteLine("here");
                

                while (await reader.ReadAsync())
                {
                    var name = reader.GetString(8);
                    var altText = reader.GetString(9);
    
                    Console.WriteLine(name);
                    Console.WriteLine(altText);
    
                    posts.Add(new BlogPost(
                        reader.GetInt32(0),     // Id
                        reader.GetString(1),    // Author
                        reader.GetString(2),    // Title
                        reader.GetString(3),    // Content
                        reader.GetDateTime(4),  // TimeStamp
                        reader.GetInt32(5),     // Likes
                        new ImageMetaData[]
                        {
                            new ImageMetaData(name, altText)
                        }
                    ));
                }

                await reader.CloseAsync();
                
                return posts;
            }
            catch (Exception ex)
            {
                await conn.CloseAsync();
                Console.WriteLine($"An error occured reading all blog posts: {ex}");
                return [];
            }
        }
    }

    public async Task<BlogPost?> InsertBlogPost(BlogPost newPost)
    {
        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                await conn.OpenAsync();

                await using var cmd = new NpgsqlCommand(Commands.InsertBlogPost, conn);

                cmd.Parameters.AddWithValue(":Author", newPost.Author);
                cmd.Parameters.AddWithValue(":Title", newPost.Title);
                cmd.Parameters.AddWithValue(":Content", newPost.Content);
                cmd.Parameters.AddWithValue(":TimeStamp", newPost.TimeStamp);
                cmd.Parameters.AddWithValue(":Likes", newPost.Likes);

                var result = cmd.ExecuteScalar();
                result = result == DBNull.Value ? null : result;

                var id = Convert.ToInt32(result);

                if (id > 0)
                {
                    var newlyInsertedBlogPost = new BlogPost(id, newPost.Author, newPost.Title, newPost.Content,
                        newPost.TimeStamp, newPost.Likes, null);
                    return newlyInsertedBlogPost;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured creating a blog post: {ex}");
                await conn.CloseAsync();
                return null;
            }
        }
    }

    public async Task<int> DeleteBlogPost(int id)
    {
        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                await conn.OpenAsync();

                using var cmd = new NpgsqlCommand(Commands.DeleteBlogPost, conn);

                cmd.Parameters.AddWithValue(":Id", id);

                var result = await cmd.ExecuteNonQueryAsync();

                return result;
            }
            catch (Exception ex)
            {
                // todo: add 400 error handling here
                Console.WriteLine($"An error occured deleting a blog post: {ex}");
                await conn.CloseAsync();
                throw;
            }
        }
    }

    public async Task<BlogPost?> PatchBlogPost(int id, BlogPost updatedPost)
    {
        using (var conn = GetIndividualConnection())
        {
            if (conn is null) throw new Exception("Connection is null");

            try
            {
                await conn.OpenAsync();

                using var cmd = new NpgsqlCommand(Commands.UpdateBlogPost, conn);

                cmd.Parameters.AddWithValue(":Author", updatedPost.Author);
                cmd.Parameters.AddWithValue(":Title", updatedPost.Title);
                cmd.Parameters.AddWithValue(":Content", updatedPost.Content);
                cmd.Parameters.AddWithValue(":TimeStamp", updatedPost.TimeStamp);
                cmd.Parameters.AddWithValue(":Likes", updatedPost.Likes);
                cmd.Parameters.AddWithValue(":Id", id);

                var result = await cmd.ExecuteNonQueryAsync();

                // handle 404 blog post not found
                return result != 1 ? null : updatedPost;
            }
            catch (Exception ex)
            {
                // todo: add 400  error handling here
                Console.WriteLine($"An error occured patching a blog post: {ex}");
                await conn.CloseAsync();
                return null;
            }
        }
    }
}