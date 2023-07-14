namespace Blog.Models;

public class BlogPostItem
{
    public int Id { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int Likes { get; set; }

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