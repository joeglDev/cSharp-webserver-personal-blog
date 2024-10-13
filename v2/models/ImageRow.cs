namespace Webserver.Models;

public class ImageRow
{
    public int Id { get; set; }
    public int BlogpostId { get; set; }
    public string? Name { get; set; }
    public byte[]? Img { get; set; }

    public ImageRow(int id, int blogpostId, string? name = null, byte[]? img = null)
    {
        Id = id;
        BlogpostId = blogpostId;
        Name = name;
        Img = img;
    }
}