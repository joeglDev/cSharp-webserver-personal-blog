namespace v2.Models;

public sealed class UserLoginRequestItem
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}