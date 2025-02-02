using Xunit;

namespace v2.utils;

public class PasswordHasherTest
{

    [Fact]
    public void RunPasswordHasher_ShouldHashAndVerifyPasswordPass()
    {
        var inputPassword = "TestPassword";
        var passwordHasher = new PasswordHasher();
        var hashedPassword = passwordHasher.RunPasswordHasher(inputPassword);

        var outcome = passwordHasher.VerifyHashedPassword(inputPassword, hashedPassword);
        Assert.True(outcome);
    }

    [Fact]
    public void RunPasswordHasher_ShouldHashAndVerifyPasswordFail()
    {
        var inputPassword = "TestPassword";
        var incorrectPassword = "incorrectPassword";
        var passwordHasher = new PasswordHasher();
        var hashedPassword = passwordHasher.RunPasswordHasher(inputPassword);

        var outcome = passwordHasher.VerifyHashedPassword(incorrectPassword, hashedPassword);
        Assert.False(outcome);
    }
}