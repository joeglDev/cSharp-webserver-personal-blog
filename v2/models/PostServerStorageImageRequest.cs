namespace v2.Models;

public class PostServerStorageImageRequest
{
    public PostServerStorageImageRequest(int blogpostId, string name, string alt, string path)
    {
        BlogpostId = blogpostId;
        Name = name;
        Alt = alt;
        Path = path;
    }

    public int BlogpostId { get; set; }
    public string Name { get; set; }
    public string Alt { get; set; }
    public string Path { get; set; }
}