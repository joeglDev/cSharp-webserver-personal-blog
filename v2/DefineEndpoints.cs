using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using v2.Controllers;
using v2.Models;

namespace v2;

public class DefineEndpoints
{
    public void AddAllEndpoints(WebApplication app)
    {
        AddUserEndpoints(app);
        AddPingEndpoints(app);
        AddBlogPostEndpoints(app);
        AddImageEndpoints(app);
        AddAntiforgeryEndpoints(app);
    }

    private static void AddUserEndpoints(WebApplication app)
    {
        // user authentication: Must get cookie from this endpoint to authorize other endpoints
        app.MapPost("/api/login",
            [AllowAnonymous] (UserLoginRequestItem userLoginRequest, HttpContext context) =>
                UserService.PostUserLogin(userLoginRequest, context)).WithTags("User");

        app.MapPost("/api/signup",
                [AllowAnonymous] (UserLoginRequestItem userLoginRequest) => UserService.PostUserSignup(userLoginRequest))
            .WithTags("User");

        app.MapGet("/api/logout", [Authorize] (context) => UserService.PostUserLogout(context)).WithTags("User");
    }

    private static void AddBlogPostEndpoints(WebApplication app)
    {
        app.MapGet("/api/posts", [Authorize] () => BlogPostService.GetAllPosts()).WithTags("Blog Posts");

        app.MapPost("/api/post", [Authorize] (BlogPost newPost) => BlogPostService.PostBlogPost(newPost)).WithTags("Blog Posts");

        app.MapDelete("/api/post/{id}", [Authorize] (int id) => BlogPostService.DeleteBlogPost(id)).WithTags("Blog Posts");

        app.MapPatch("/api/post/{id}",
                [Authorize] (int id, BlogPost updatedBlogPost) => BlogPostService.PatchBlogPost(id, updatedBlogPost))
            .WithTags("Blog Posts");
    }

    private static void AddImageEndpoints(WebApplication app)
    {
        app.MapGet("/api/server_storage/image", [Authorize] (int id) => ServerStorageImageService.GetImageFile(id))
            .WithTags("Server storage images");

        app.MapPost("/api/server_storage/image/{id}",
                [Authorize] (int id, string name, string alt, IFormFile imageFile) =>
                    ServerStorageImageService.PostImage(id, name, alt, imageFile))
            .WithTags("Server storage images");

        app.MapDelete("/api/server_storage/image/{id}", [Authorize] (int id) => ServerStorageImageService.DeleteImage(id)).WithTags("Server storage images");
    }

    private static void AddPingEndpoints(WebApplication app)
    {
        app.MapGet("/api/ping", () => "pong").WithTags("General");

        app.MapGet("/api/authorised-ping", [Authorize] () => PingController.AuthorisedPing()).WithTags("General");

        app.MapGet("/api/author", () => GetAuthorItemService.GetAuthorItem("Joe Gilbert", "joeglDev")).WithTags("General");
    }

    private static void AddAntiforgeryEndpoints(WebApplication app)
    {
        app.MapGet("/api/generate-antiforgery", [Authorize] (HttpContext context, IAntiforgery antiforgery) => AntiforgeryController.GetToken(context, antiforgery)).WithTags("Antiforgery");
    }
}