
namespace Webserver.Controllers;

using System.Collections.Generic;
using Db;
using Webserver.Models;


public class ImageService
{
    private static readonly ImageDatabaseService _service = new ImageDatabaseService();

    public static async Task<List<ImageRow>> GetAllImages()
    {
        List<ImageRow> AllImages = await _service.GetAllImages();

        return AllImages;
    }

    public static async Task<IResult> GetImage(int Id)
    {
        ImageRow? Image = await _service.GetImageById(Id);
        if (Image is not null)
        {
            return Results.Ok(Image);
        }
        else
        {
            return Results.BadRequest();
        }
    }

    private static async Task<byte[]> ConvertIFormFileToByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public static async Task<IResult> PostImage(int Id, IFormFile ImageFile)
    {
        string Name = ImageFile.Name;
        byte[] fileBytes = await ConvertIFormFileToByteArray(ImageFile);

        // note does not need Id field
        ImageRow NewImage = new ImageRow(Id, Id, Name, fileBytes);

        bool InsertSucceeded = await _service.InsertImage(NewImage);
        if (InsertSucceeded)
        {
            return Results.Created("/api/image" + NewImage.Id, NewImage);
        }
        else
        {
            return Results.BadRequest();
        }
    }

    public static async Task<IResult> DeleteImage(int Id)
    {
        bool DeleteSucceeded = await _service.DeleteImage(Id);

        if (DeleteSucceeded)
        {
            return Results.NoContent();
        }
        else
        {
            return Results.BadRequest();
        }
    }
};
