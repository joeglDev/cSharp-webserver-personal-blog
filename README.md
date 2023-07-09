# cSharp-webserver-personal-blog
A web server running C# / dotNET to handle API requests for a personal blogging website.

## Run locally and get Documentation

When running in dotNET press `CTRL + F5` and navigate to `https://localhost:7234/swagger/index.html`.

## Documentation

```
GET /author 
Returns: 
{
    Name: Joe Gilbert
    Github: joegldev
}

```

## Dependencies
    Microsoft.AspNetCore.OpenApi Version: 7.0.8
    Microsoft.EntityFrameworkCore.InMemory Version: 7.0.8
    Swashbuckle.AspNetCore Version: 6.5.0
