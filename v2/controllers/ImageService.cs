
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
};
