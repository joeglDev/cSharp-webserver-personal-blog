namespace Webserver.Controllers;

using System.Collections.Generic;
using Webserver.Models;

public class BlogPostService {
    public static List<BlogPost> GetAllPosts() {
        List<BlogPost> AllBlogPosts = [];

        // note
        // replace this with a database query fetching the post from the database
        BlogPost examplePost = new(1, "Hiroji", "Cats are cool!", "This is an example blog post to tell you that my cat Bennet is one cool cat. A tuxedo cat to be specific! ^w^", DateTime.Now, 5);
        AllBlogPosts.Add(examplePost);

        return AllBlogPosts;
        
    }
};
