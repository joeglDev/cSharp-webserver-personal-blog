using v2.Db;
using v2.Models;

namespace v2.Controllers;

public class ServerStorageImageService
{
    private static readonly ServerStorageImageDatabaseService Service = new();

    // TODO: does not return alt text so will need a endpoint to get this
    public static async Task<IResult> GetImageFile(int id)
    {
        var imageMetaData = await Service.GetImageFile(id);

        if (imageMetaData is null) return Results.NotFound();

        try
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "public");
            var fullPath = Path.Combine(basePath, imageMetaData.Path);
            Console.WriteLine(fullPath);
            var imageStream = File.OpenRead(fullPath);

            return Results.File(
                imageStream,
                "image/jpeg",
                imageMetaData.Name,
                enableRangeProcessing: true
            );
        }
        catch (FileNotFoundException)
        {
            return Results.NotFound("File not found");
        }
    }

    public static async Task<IResult> PostImage(int blogpostId, string name, string alt, IFormFile imageFile)
    {
        // insert into server_storage_images table
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "public");
        var fileCount = Directory.GetFiles(basePath).Length + 1;
        var path = $"image_{fileCount}";
        var newImageRequest = new PostServerStorageImageRequest(blogpostId, name, alt, path);
        var insertImageMetaData = await Service.InsertImage(newImageRequest);
        
        Console.WriteLine(insertImageMetaData);
        if (insertImageMetaData is false) {
            return Results.NotFound();
        }
        
        // save file to disk
        var filePath = Path.Combine(basePath, path);
        Console.WriteLine(filePath);
        
        await using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await  imageFile.CopyToAsync(fileStream);
        }
        
        return Results.Created("/api/server_storage/image" + blogpostId, newImageRequest);
    }
}