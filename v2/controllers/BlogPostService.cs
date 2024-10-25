namespace Webserver.Controllers;

using System.Collections.Generic;
using Db;
using Models;
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
        bool insertSucceeded = await Service.InsertBlogPost(newPost);
        if (insertSucceeded)
        {
            return Results.Created("/api/posts/" + newPost.Id, newPost);
        }
        else
        {
            return Results.BadRequest();
        }
    }

    public static async Task<IResult> DeleteBlogPost(int id)
    {
        bool deleteSucceeded = await Service.DeleteBlogPost(id);

        if (deleteSucceeded)
        {
            return Results.NoContent();
        }
        else
        {
            return Results.BadRequest();
        }
    }

    public static async Task<IResult> PatchBlogPost(int id, BlogPost updatedBlogPost)
    {
        BlogPost? patchResponse = await Service.PatchBlogPost(id, updatedBlogPost);

        if (patchResponse is not null)
        {
            return Results.Ok(patchResponse);
        }
        else
        {
            return Results.BadRequest();
        }
    }
};
