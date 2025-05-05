using v2.Db;
using v2.Models;

namespace v2.Controllers;

[Obsolete("Use ServerStorageImageService instead")]
public class ImageService
{
    private static readonly ImageDatabaseService Service = new();

    public static async Task<List<ImageRow>> GetAllImages()
    {
        var allImages = await Service.GetAllImages();

        return allImages;
    }

    public static async Task<IResult> GetImage(int id)
    {
        var image = await Service.GetImageById(id);
        if (image is not null) return Results.Ok(image);

        if (image is null) return Results.NotFound();

        return Results.InternalServerError();
    }

    private static async Task<byte[]> ConvertIFormFileToByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public static async Task<IResult> PostImage(int id, IFormFile imageFile)
    {
        var name = imageFile.Name;
        var fileBytes = await ConvertIFormFileToByteArray(imageFile);

        // note does not need Id field
        var newImage = new ImageRow(id, id, name, fileBytes);

        var insertSucceeded = await Service.InsertImage(newImage);
        if (insertSucceeded) return Results.Created("/api/image" + newImage.Id, newImage);

        return Results.BadRequest();
    }

    public static async Task<IResult> DeleteImage(int id)
    {
        var deleteSucceeded = await Service.DeleteImage(id);

        if (deleteSucceeded is true) return Results.NoContent();

        if (deleteSucceeded is null) return Results.NotFound();

        return Results.BadRequest();
    }
}