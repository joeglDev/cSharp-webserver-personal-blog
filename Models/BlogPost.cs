namespace Blog.Models;

public class BlogPostItem
{
    public int Id { get; set; }
    public string Author { get; set; } = "unassigned author";
    public string Title { get; set; } = "unassigned title";
    public string Content { get; set; } = "unassigned content";
    public int Likes { get; set; }

    public DateTime TimeStamp {get; set;}

      // Parameterless constructor for Entity Framework
    public BlogPostItem()
    {
    }

    // Constructor
    public BlogPostItem(int id, string author, string title, string content, int likes)
    {
        Id = id;
        Author = author;
        Title = title;
        Content = content;
        Likes = likes;
    }

}