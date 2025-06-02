namespace v2.Controllers;

public static class PingController
{
    public static IResult AuthorisedPing()
    {
        return Results.Ok("Pong");
    }
}