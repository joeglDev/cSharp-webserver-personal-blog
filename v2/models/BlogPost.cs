namespace Webserver.Models;

public class BlogPost {
    public int Id { get; set; }
    public string Author { get; set; } = "unassigned author";
    public string Title { get; set; } = "unassigned title";
    public string Content { get; set; } = "unassigned content";
    public DateTime TimeStamp {get; set;}
    public int Likes {get; set;} = 0;

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