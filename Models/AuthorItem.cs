namespace Blog.Models;

//A model is a set of classes that represent the data that the app manages.
public class AuthorItem
{
    public string? Name {get; set; }
    public string Github { get; set; }

     public AuthorItem(string nameString, string githubString)
    {
        this.Name = nameString;
        this.Github = githubString;
    }
} 