namespace v2.Controllers;

using v2.Db;
using v2.Models;
using Microsoft.AspNetCore.Http;

public class BlogPostService
{
    private static readonly BlogPostDatabaseService Service = new BlogPostDatabaseService();

    public static async Task<List<BlogPost>> GetAllPosts()
    {
        List<BlogPost> allBlogPosts = await Service.GetAllBlogPosts();

        return allBlogPosts;

    }

    public static async Task<IResult> PostBlogPost(BlogPost newPost)
    {
        var response = await Service.InsertBlogPost(newPost);
        if (response is null)
        {
            return Results.BadRequest();
        }
        else
        {
            return Results.Created("/api/posts/" + newPost.Id, response);
        }
    }

    public static async Task<IResult> DeleteBlogPost(int id)
    {
        var result = await Service.DeleteBlogPost(id);

        if (result == 1)
        {
            return Results.NoContent();
        }
        else if (result == 0)
        {
            return Results.NotFound();
        }
        else
        {
            return Results.InternalServerError();
        }
    }

    public static async Task<IResult> PatchBlogPost(int id, BlogPost updatedBlogPost)
    {
        var patchResponse = await Service.PatchBlogPost(id, updatedBlogPost);

        if (patchResponse is not null)
        {
            return Results.Ok(patchResponse);
        }
        else
        {
            return Results.NotFound();
        }
    }
};
