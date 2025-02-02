using v2.Models;

namespace v2.Controllers;

public class UserService
{
    public static IResult PostUserLogin(UserLoginRequestItem request)
    {
        // 1. DONE - Seeder func should create user table then add username and hash salted password
        // 2. Check against username and password in database 
        // - DONE - Insert username and hashed and salted password into database
        // - hash and salt request password and check against username
        // if match than return token and username
        // if not reject

        return Results.Ok();
    }
}