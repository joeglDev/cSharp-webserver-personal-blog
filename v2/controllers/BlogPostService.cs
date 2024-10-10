namespace Webserver.Controllers;

using System.Collections.Generic;
using Db;
using Webserver.Models;

public class BlogPostService
{
    public static async Task<List<BlogPost>> GetAllPosts()
    {
        var Service = new DatabaseService();
        List<BlogPost> AllBlogPosts = await Service.GetAllBlogPosts();

        return AllBlogPosts;

    }
};
