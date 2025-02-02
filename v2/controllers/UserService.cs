using v2.Models;
using v2.Db;
using v2.utils;

namespace v2.Controllers;

public class UserService
{
    private static readonly UserDatabaseService Service = new UserDatabaseService();
    public static async Task<IResult> PostUserLogin(UserLoginRequestItem request)
    {
        // 1. DONE - Seeder func should create user table then add username and hash salted password
        // 2. Check against username and password in database 
        // - DONE - Insert username and hashed and salted password into database
        // - hash and salt request password and check against username ie get password from db by username string
        // if match than return token and username
        // if not reject
        
        // insert new userendpoint
        
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
        return Results.Ok();
    }
}