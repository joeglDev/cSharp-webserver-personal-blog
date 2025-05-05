namespace v2.Models;

public class ImageRow
{
    public ImageRow(int id, int blogpostId, string name, byte[] img)
    {
        Id = id;
        BlogpostId = blogpostId;
        Name = name;
        Img = img;
    }

    public int Id { get; set; }
    public int BlogpostId { get; set; }
    public string Name { get; set; }
    public byte[] Img { get; set; }
}