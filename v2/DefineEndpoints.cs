using Microsoft.AspNetCore.Authorization;
using v2.Controllers;
using v2.Models;
using v2.utils;

namespace v2;

/*
 * Todo:
 * Consider adding a GET specific blog post by id endpoint
 */

public class DefineEndpoints
{
    private static readonly bool isDevelopment = GetEnvVariables()["ENVIRONMENT"] is "development";

    public void AddAllEndpoints(WebApplication app)
    {
        Console.WriteLine($"Is development environment: {isDevelopment}");

        AddUserEndpoints(app);
        AddPingEndpoints(app);
        AddBlogPostEndpoints(app);
        AddImageEndpoints(app);
    }

    private static void AddUserEndpoints(WebApplication app)
    {
        if (isDevelopment)
        {
            // user authentication: Must get cookie from this endpoint to authorize other endpoints
            app.MapPost("/api/login",
                [AllowAnonymous] (UserLoginRequestItem userLoginRequest, HttpContext context) =>
                    UserService.PostUserLogin(userLoginRequest, context)).WithTags("User");

            app.MapPost("/api/signup",
                    [AllowAnonymous] (UserLoginRequestItem userLoginRequest) => UserService.PostUserSignup(userLoginRequest))
                .WithTags("User");

            app.MapGet("/api/logout", [Authorize] (HttpContext context) => UserService.PostUserLogout(context)).WithTags("User");
        }
    }

    private static void AddBlogPostEndpoints(WebApplication app)
    {
        if (isDevelopment)
        {
            app.MapPost("/api/post", [Authorize] (BlogPost newPost) => BlogPostService.PostBlogPost(newPost)).WithTags("Blog Posts");

            app.MapDelete("/api/post/{id}", [Authorize] (int id) => BlogPostService.DeleteBlogPost(id)).WithTags("Blog Posts");

            app.MapPatch("/api/post/{id}",
                    [Authorize] (int id, BlogPost updatedBlogPost) => BlogPostService.PatchBlogPost(id, updatedBlogPost))
                .WithTags("Blog Posts");
        }

        app.MapGet("/api/posts", [AllowAnonymous] () => BlogPostService.GetAllPosts()).WithTags("Blog Posts").Produces<BlogPost[]>();

        app.MapGet("/api/posts/{id}", [AllowAnonymous] (int id) => BlogPostService.GetBlogPost(id)).WithTags("Blog Posts").Produces<BlogPost>();
    }

    private static void AddImageEndpoints(WebApplication app)
    {
        if (isDevelopment)
        {
            // Todo: implement antiforgery
            app.MapPost("/api/server_storage/image/{id}",
                    [Authorize] (int id, string name, string alt, IFormFile imageFile) =>
                        ServerStorageImageService.PostImage(id, name, alt, imageFile))
                .WithTags("Server storage images").DisableAntiforgery();

            app.MapDelete("/api/server_storage/image/{id}", [Authorize] (int id) => ServerStorageImageService.DeleteImage(id)).WithTags("Server storage images");
        }

        app.MapGet("/api/server_storage/image/{id}", [AllowAnonymous] (int id) => ServerStorageImageService.GetImageFile(id))
            .WithTags("Server storage images");
    }

    private static void AddPingEndpoints(WebApplication app)
    {
        if (isDevelopment)
        {
            app.MapGet("/api/authorised-ping", [Authorize] () => PingController.AuthorisedPing()).WithTags("General");
        }
        app.MapGet("/api/ping", () => "pong").WithTags("General");

        app.MapGet("/api/author", () => GetAuthorItemService.GetAuthorItem("Joe Gilbert", "joeglDev")).WithTags("General");
    }

    // TODO: Abstract this private method and the identically and method in DatabaseAbstract to a utility class.
    private static Dictionary<string, string?> GetEnvVariables()
    {
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, ".env");
        DotEnv.Load(dotenv);

        var envVars = new Dictionary<string, string?>
        {
            ["ENVIRONMENT"] = Environment.GetEnvironmentVariable("ENVIRONMENT"),
        };

        return envVars;
    }
}