using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
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

        if (insertImageMetaData is false) return Results.NotFound();

        // save file to disk
        var filePath = Path.Combine(basePath, path);

        await using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return Results.Created("/api/server_storage/image" + blogpostId, newImageRequest);
    }

    public static async Task<IResult> DeleteImage(int id)
    {
        // get image metadata from database for rollback if necessary
        var imageMetaData = await Service.GetImageFile(id);
        if (imageMetaData is null) return Results.NotFound();

        //  delete image metadata
        var deleteImage = await Service.DeleteImage(id);
        if (deleteImage is false) return Results.NotFound();

        // if successful delete file
        try
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "public") + $"/image_{id}";
            Console.WriteLine($"Deleting image: {fullPath}");
            File.Delete(fullPath);

            return Results.NoContent();
        }
        catch (FileNotFoundException)
        {
            return await RestoreImageMetaData(imageMetaData);
        }
    }

    /*
     If image metadata has been deleted successfully but there is an error when deleting the image from file storage;
     this can result in a conflict between expected number of files in storage and rows in the image metadata table in database.
     To prevent this from occurring; insert the image metadata back into the metadata table.
     */
    private static async Task<IResult> RestoreImageMetaData(ServerStorageImage imageMetaData)
    {
        var (blogpostId, name, alt, path) = imageMetaData;
        var newImageRequest = new PostServerStorageImageRequest(blogpostId, name, alt, path);
        var insertImageMetaData = await Service.InsertImage(newImageRequest);

        if (insertImageMetaData is false) return Results.Conflict("The file metadata has been deleted but the file still exists. Database mismatch may occur.");

        return Results.NotFound("File not found. Deleted metadata has been restored.");
    }
}