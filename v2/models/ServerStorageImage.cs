namespace v2.Models;

public class ServerStorageImage
{
    public ServerStorageImage(int id, int blogpostId, string name, string alt, string path)
    {
        Id = id;
        BlogpostId = blogpostId;
        Name = name;
        Alt = alt;
        Path = path;
    }

    public int Id { get; set; }
    public int BlogpostId { get; set; }
    public string Name { get; set; }
    public string Alt { get; set; }
    public string Path { get; set; }

    public void Deconstruct(out int blogpostId, out string name, out
        string alt, out string path)
    {
        blogpostId = BlogpostId;
        name = Name;
        alt = Alt;
        path = Path;
    }
}