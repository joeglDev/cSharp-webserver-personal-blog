using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using v2.Models;
using v2.Db;
using v2.utils;

namespace v2.Controllers;

public class UserService : ControllerBase
{

    private static readonly UserDatabaseService Service = new UserDatabaseService();
    public static async Task<IResult> PostUserLogin(UserLoginRequestItem request, HttpContext context)
    {
        var dbHashedPassword = await Service.GetPasswordByUsername(request.Username);

        // handle 404
        if (dbHashedPassword is null)
        {
            return Results.NotFound();
        }

        // verify password against hashedPassword
        var passwordHasher = new PasswordHasher();
        var doesPasswordMatchHash = passwordHasher.VerifyHashedPassword(request.Password, dbHashedPassword);

        // password is incorrect
        if (!doesPasswordMatchHash)
        {
            return Results.Unauthorized();
        }

        // authorise user
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
        identity.AddClaim(new Claim(ClaimTypes.Name, request.Username));
        identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            IsPersistent = true,
            IssuedUtc = DateTimeOffset.Now
        };

        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);


        return Results.Ok();
    }

    public static async Task PostUserLogout(HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public static async Task<IResult> PostUserSignup(UserLoginRequestItem request)
    {
        var result = await Service.InsertNewUser(request.Username, request.Password);

        if (result > 0)
        {
            return Results.Ok();
        }
        else if (result == 0)
        {
            return Results.BadRequest("This username already exists.");
        }

        return Results.InternalServerError("An error occured inserting the new user");
    }
}