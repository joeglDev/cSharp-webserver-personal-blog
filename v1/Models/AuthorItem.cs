namespace Blog.Models;

//A model is a set of classes that represent the data that the app manages.
public class AuthorItem
{
    public AuthorItem(string nameString, string githubString)
    {
        Name = nameString;
        Github = githubString;
    }

    public string? Name { get; set; }
    public string Github { get; set; }
}