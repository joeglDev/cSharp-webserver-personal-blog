namespace v2.Models;

public class BlogPost
{
    public int Id { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime TimeStamp { get; set; }
    public int Likes { get; set; }

    // Constructor
    public BlogPost(int id, string author, string title, string content, DateTime timeStamp, int likes)
    {
        Id = id;
        Author = author;
        Title = title;
        Content = content;
        TimeStamp = timeStamp;
        Likes = likes;
    }
}