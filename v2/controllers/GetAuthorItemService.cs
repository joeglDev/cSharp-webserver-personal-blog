namespace v2.Controllers;

using v2.Models;

public class GetAuthorItemService
{
    public static AuthorItem GetAuthorItem(string name, string githubUsername)
    {
        var myAuthorDetails = new AuthorItem { Name = name, GithubUsername = githubUsername };
        return myAuthorDetails;
    }
};

