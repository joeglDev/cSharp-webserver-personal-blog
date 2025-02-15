
namespace v2.Controllers;

using System.Collections.Generic;
using Db;
using Models;


public class ImageService
{
    private static readonly ImageDatabaseService Service = new ImageDatabaseService();

    public static async Task<List<ImageRow>> GetAllImages()
    {
        List<ImageRow> allImages = await Service.GetAllImages();

        return allImages;
    }

    public static async Task<IResult> GetImage(int id)
    {
        ImageRow? image = await Service.GetImageById(id);
        if (image is not null)
        {
            return Results.Ok(image);
        }
        else if (image is null)
        {
            return Results.NotFound();
        }
        else
        {
            return Results.InternalServerError();
        }
    }

    private static async Task<byte[]> ConvertIFormFileToByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public static async Task<IResult> PostImage(int id, IFormFile imageFile)
    {
        string name = imageFile.Name;
        byte[] fileBytes = await ConvertIFormFileToByteArray(imageFile);

        // note does not need Id field
        var newImage = new ImageRow(id, id, name, fileBytes);

        bool insertSucceeded = await Service.InsertImage(newImage);
        if (insertSucceeded)
        {
            return Results.Created("/api/image" + newImage.Id, newImage);
        }
        else
        {
            return Results.BadRequest();
        }
    }

    public static async Task<IResult> DeleteImage(int id)
    {
        var deleteSucceeded = await Service.DeleteImage(id);

        if (deleteSucceeded is true)
        {
            return Results.NoContent();
        }
        else if (deleteSucceeded is null)
        {
            return Results.NotFound();
        }
        else
        {
            return Results.BadRequest();
        }
    }
};
