namespace v2.Controllers;

public class PingController
{
    public static IResult AuthorisedPing()
    {
        return Results.Ok("Pong");
    }
}