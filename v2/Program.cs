using Webserver.Controllers;
using Db;
using Webserver.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

var seeder = new DatabaseSeeder();
await seeder.SeedDbAsync();

app.MapGet("/ping", () => "pong");

app.MapGet("/author", () => GetAuthorItemService.GetAuthorItem("Joe Gilbert", "joeglDev"));

app.MapGet("/posts", () => BlogPostService.GetAllPosts());

app.MapPost("/post", (BlogPost NewPost) => BlogPostService.PostBlogPost(NewPost));

app.MapDelete("/post/{id}", (int id) => BlogPostService.DeleteBlogPost(id));

app.Run();