namespace Webserver.Controllers;

using System.Collections.Generic;
using Db;
using Webserver.Models;
using Microsoft.AspNetCore.Http;
using System.Net;

public class BlogPostService
{
    private static readonly DatabaseService _service = new DatabaseService();

    public static async Task<List<BlogPost>> GetAllPosts()
    {
        List<BlogPost> AllBlogPosts = await _service.GetAllBlogPosts();

        return AllBlogPosts;

    }

    //todo: should return HTTP 204
    public static async Task<IResult> PostBlogPost(BlogPost NewPost)
    {
        bool InsertSucceeded = await _service.InsertBlogPost(NewPost);
        if (InsertSucceeded)
        {
            return Results.Created();
        }
        else
        {
            return Results.BadRequest();
        }
    }

    //todo: should return HTTP 201
    public static async Task<IResult> DeleteBlogPost(int id)
    {
        bool DeleteSucceeded = await _service.DeleteBlogPost(id);

        if (DeleteSucceeded)
        {
            return Results.NoContent();
        }
        else
        {
            return Results.BadRequest();
        }
    }

    public static async Task<IResult> PatchBlogPost(int id, BlogPost UpdatedBlogPost)
    {
        BlogPost? PatchResponse = await _service.PatchBlogPost(id, UpdatedBlogPost);

        if (PatchResponse is not null)
        {
            return Results.Ok(PatchResponse);
        }
        else
        {
            return Results.BadRequest();
        }
    }
};
