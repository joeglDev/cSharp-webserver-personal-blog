using Microsoft.EntityFrameworkCore;
using Blog.Models;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:3000");
                         policy.WithMethods("GET", "POST", "DELETE", "PUT", "PATCH");
                          policy.WithHeaders("authorization", "accept", "content-type", "origin");
                      }
                     );
});

// Add services to the container.

builder.Services.AddControllers();

//create depedency injection db for blog posts
builder.Services.AddDbContext<BlogPostContext>(opt =>
    opt.UseInMemoryDatabase("BlogPostList"));
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//cors
app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
