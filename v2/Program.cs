using Webserver.Controllers;
using Db;
using Webserver.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalHost",
        builder => builder
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

app.MapPost("/api/post", (BlogPost NewPost) => BlogPostService.PostBlogPost(NewPost)).WithTags("Blog Posts");

app.MapDelete("/api/post/{id}", (int id) => BlogPostService.DeleteBlogPost(id)).WithTags("Blog Posts");

app.MapPatch("/api/post/{id}", (int id, BlogPost UpdatedBlogPost) => BlogPostService.PatchBlogPost(id, UpdatedBlogPost)).WithTags("Blog Posts");

// images
app.MapGet("/api/images", () => ImageService.GetAllImages()).WithTags("Images");

app.MapGet("/api/image/{id}", (int id) => ImageService.GetImage(id)).WithTags("Images");

app.Run();