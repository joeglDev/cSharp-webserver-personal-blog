using Microsoft.AspNetCore.Antiforgery;

namespace v2.Controllers;

public class AntiforgeryController
{
    public static IResult GetToken(HttpContext context, IAntiforgery antiforgery)
    {

        var tokenSet = antiforgery.GetAndStoreTokens(context);

        return Results.Ok(tokenSet.RequestToken);
    }
}