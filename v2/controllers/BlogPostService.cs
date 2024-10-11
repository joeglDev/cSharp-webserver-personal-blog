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
    public static async Task<object> PostBlogPost(BlogPost NewPost)
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
};
