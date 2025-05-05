using v2.Db;

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
}