namespace Blog.Models;

public class LikedByItem
{
    // Parameterless constructor for Entity Framework
    public LikedByItem()
    {
    }

    public LikedByItem(string name)
    {
        UserName = name;
    }

    public int LikedByItemId { get; set; } // Primary key

    //  public int UserId { get; set; }
    public string? UserName { get; set; }
}

public class BlogPostItem
{
    // Parameterless constructor for Entity Framework
    public BlogPostItem()
    {
    }

    // Constructor
    public BlogPostItem(int id, string author, string title, string content)
    {
        Id = id;
        Author = author;
        Title = title;
        Content = content;
    }

    public int Id { get; set; }
    public string Author { get; set; } = "unassigned author";
    public string Title { get; set; } = "unassigned title";
    public string Content { get; set; } = "unassigned content";

    public DateTime TimeStamp { get; set; }

    public List<LikedByItem> Likes { get; set; } = new();
}