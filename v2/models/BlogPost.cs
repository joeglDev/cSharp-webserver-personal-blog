namespace v2.Models;

public class BlogPost
{
    // Constructor
    public BlogPost(int id, string author, string title, string content, DateTime timeStamp, int likes, ImageMetaData[]? images)
    {
        Id = id;
        Author = author;
        Title = title;
        Content = content;
        TimeStamp = timeStamp;
        Likes = likes;
        Images = images;
    }

    public int Id { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime TimeStamp { get; set; }
    public int Likes { get; set; }
    public ImageMetaData[]? Images { get; set; }
}

public class ImageMetaData
{
    public string Name { get; set; }
    public string AltText { get; set; }

    public ImageMetaData(string name, string altText)
    {
        Name = name;
        AltText = altText;
    }
}