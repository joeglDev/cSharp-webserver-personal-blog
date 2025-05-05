using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly ILogger<AuthorController> _logger;

    public AuthorController(ILogger<AuthorController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetAuthor")]
    public AuthorItem Get()
    {
        return new AuthorItem("Joe Gilbert", "joegldev");
    }
}