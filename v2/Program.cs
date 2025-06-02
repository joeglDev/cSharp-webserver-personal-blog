using Microsoft.AspNetCore.Authentication.Cookies;
using v2;
using v2.Db;

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

// define endpoints
var defineEndpoints = new DefineEndpoints();
defineEndpoints.AddAllEndpoints(app);

app.Run();