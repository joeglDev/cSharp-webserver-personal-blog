namespace Webserver.Controllers;

using Webserver.Models;

public class GetAuthorItemService {
    public static AuthorItem GetAuthorItem(string name, string githubUsername)
    {
        AuthorItem myAuthorDetails = new AuthorItem { Name = name, GithubUsername = githubUsername };
        return myAuthorDetails;
    }
};

