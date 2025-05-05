using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

// User auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(180);
        options.SlidingExpiration = true;
        options.LoginPath = "/api/login";
        options.LogoutPath = "/api/logout";
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });


builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowLocalHost");

// User auth
app.UseAuthorization();

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict
};

app.UseCookiePolicy(cookiePolicyOptions);

// Seed database
var seeder = new DatabaseSeeder();
await seeder.SeedDbAsync();

// user authentication: Must get cookie from this endpoint to authorize other endpoints
app.MapPost("/api/login",
    [AllowAnonymous](UserLoginRequestItem userLoginRequest, HttpContext context) =>
        UserService.PostUserLogin(userLoginRequest, context)).WithTags("User");

app.MapPost("/api/signup",
        [AllowAnonymous](UserLoginRequestItem userLoginRequest) => UserService.PostUserSignup(userLoginRequest))
    .WithTags("User");

app.MapGet("/api/logout", [Authorize](context) => UserService.PostUserLogout(context)).WithTags("User");

// blogposts 
app.MapGet("/api/ping", () => "pong").WithTags("General");

app.MapGet("/api/author", () => GetAuthorItemService.GetAuthorItem("Joe Gilbert", "joeglDev")).WithTags("General");

app.MapGet("/api/posts", [Authorize]() => BlogPostService.GetAllPosts()).WithTags("Blog Posts");

app.MapPost("/api/post", [Authorize](BlogPost newPost) => BlogPostService.PostBlogPost(newPost)).WithTags("Blog Posts");

app.MapDelete("/api/post/{id}", [Authorize](int id) => BlogPostService.DeleteBlogPost(id)).WithTags("Blog Posts");

app.MapPatch("/api/post/{id}",
        [Authorize](int id, BlogPost updatedBlogPost) => BlogPostService.PatchBlogPost(id, updatedBlogPost))
    .WithTags("Blog Posts");

// images
app.MapGet("/api/images", [Authorize]() => ImageService.GetAllImages()).WithTags("Images");

app.MapGet("/api/image/{id}", [Authorize](int id) => ImageService.GetImage(id)).WithTags("Images");

// Todo: implement antiforgery
app.MapPost("/api/image/{id}", [Authorize](int id, IFormFile imageFile) => ImageService.PostImage(id, imageFile))
    .WithTags("Images").DisableAntiforgery();

app.MapDelete("/api/image/{id}", [Authorize](int id) => ImageService.DeleteImage(id)).WithTags("Images");

app.Run();