using v2.Db;
using v2.Models;

namespace v2.Controllers;

public class BlogPostService
{
    private static readonly BlogPostDatabaseService Service = new();

    public static async Task<IResult> GetAllPosts()
    {
        var allBlogPosts = await Service.GetAllBlogPosts();

        if (allBlogPosts is null)
        {
            return Results.InternalServerError("An unhandled error occured please contact the developer.");
        }

        return Results.Ok(allBlogPosts);
    }

    public static async Task<IResult> PostBlogPost(BlogPost newPost)
    {
        var response = await Service.InsertBlogPost(newPost);
        if (response is null) return Results.BadRequest();

        return Results.Created("/api/posts/" + newPost.Id, response);
    }

    public static async Task<IResult> DeleteBlogPost(int id)
    {
        var result = await Service.DeleteBlogPost(id);

        if (result == 1) return Results.NoContent();

        if (result == 0) return Results.NotFound();

        return Results.InternalServerError();
    }

    public static async Task<IResult> PatchBlogPost(int id, BlogPost updatedBlogPost)
    {
        var patchResponse = await Service.PatchBlogPost(id, updatedBlogPost);

        if (patchResponse is not null) return Results.Ok(patchResponse);

        return Results.NotFound();
    }
}