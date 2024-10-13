using Webserver.Controllers;
using Db;
using Webserver.Models;

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
app.MapGet("/api/ping", () => "pong");

app.MapGet("/api/author", () => GetAuthorItemService.GetAuthorItem("Joe Gilbert", "joeglDev"));

app.MapGet("/api/posts", () => BlogPostService.GetAllPosts());

app.MapPost("/api/post", (BlogPost NewPost) => BlogPostService.PostBlogPost(NewPost));

app.MapDelete("/api/post/{id}", (int id) => BlogPostService.DeleteBlogPost(id));

app.MapPatch("/api/post/{id}", (int id, BlogPost UpdatedBlogPost) => BlogPostService.PatchBlogPost(id, UpdatedBlogPost));

// images


app.Run();