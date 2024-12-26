using v2.Controllers;
using v2.Db;
using v2.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalHost",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .WithMethods("GET", "POST", "PATCH", "DELETE")
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowLocalHost");

var seeder = new DatabaseSeeder();
await seeder.SeedDbAsync();

// blogposts 
app.MapGet("/api/ping", () => "pong").WithTags("General");

app.MapGet("/api/author", () => GetAuthorItemService.GetAuthorItem("Joe Gilbert", "joeglDev")).WithTags("General");

app.MapGet("/api/posts", () => BlogPostService.GetAllPosts()).WithTags("Blog Posts");

app.MapPost("/api/post", (BlogPost newPost) => BlogPostService.PostBlogPost(newPost)).WithTags("Blog Posts");

app.MapDelete("/api/post/{id}", (int id) => BlogPostService.DeleteBlogPost(id)).WithTags("Blog Posts");

app.MapPatch("/api/post/{id}", (int id, BlogPost updatedBlogPost) => BlogPostService.PatchBlogPost(id, updatedBlogPost)).WithTags("Blog Posts");

// images
app.MapGet("/api/images", () => ImageService.GetAllImages()).WithTags("Images");

app.MapGet("/api/image/{id}", (int id) => ImageService.GetImage(id)).WithTags("Images");

// Todo: implement antiforgery
app.MapPost("/api/image/{id}", (int id, IFormFile imageFile) => ImageService.PostImage(id, imageFile)).WithTags("Images").DisableAntiforgery();

app.MapDelete("/api/image/{id}", (int id) => ImageService.DeleteImage(id)).WithTags("Images");

app.Run();