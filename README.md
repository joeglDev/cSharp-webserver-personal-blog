# cSharp-webserver-personal-blog
A web server running C# / dotNET to handle API requests for a personal blogging website.

## Run locally and get Documentation

When running in dotNET press `CTRL + F5` and navigate to `https://localhost:7234/swagger/index.html`.

## Documentation

- Returns the developer who created this app's details.
```
GET /author 
Returns: 
{
    Name: Joe Gilbert
    Github: joegldev
}

```

- Returns every blog post on the system (does not filter by user)
/* TODO:add images */
```
GET /blogs
Returns:
{   
    Id: Number
    Author: String
    Title: String
    Content: String
    Likes: Number
}

```

## Dependencies
    Microsoft.AspNetCore.OpenApi Version: 7.0.8
    Microsoft.EntityFrameworkCore.InMemory Version: 7.0.8
    Swashbuckle.AspNetCore Version: 6.5.0
