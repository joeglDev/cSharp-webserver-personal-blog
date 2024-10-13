
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
};
