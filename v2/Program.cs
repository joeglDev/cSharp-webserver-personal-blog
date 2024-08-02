using Webserver.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/ping", () => "pong");

app.MapGet("/author", () => GetAuthorItemService.GetAuthorItem("Joe Gilbert", "joeglDev"));

app.MapGet("/posts", () => BlogPostService.GetAllPosts());

app.Run();
