namespace v2.utils;

public class PasswordHasher
{
    public string RunPasswordHasher(string password)
    {
        var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
        return hashedPassword;
    }

    public bool VerifyHashedPassword(string password, string hashedPassword)
    {
        var verified = BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        return verified;
    }
}