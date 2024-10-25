using Npgsql;
using utils;

namespace Db;

public abstract class DatabaseAbstract
{
    public NpgsqlConnection? _connection;
    public DatabaseCommands _commands = new DatabaseCommands();

    private static Dictionary<string, string?> GetEnvVariables()
    {
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, ".env");
        DotEnv.Load(dotenv);

        var EnvVars = new Dictionary<string, string?>
        {
            ["HOST"] = Environment.GetEnvironmentVariable("HOST"),
            ["DATABASE"] = Environment.GetEnvironmentVariable("DATABASE"),
            ["USERNAME"] = Environment.GetEnvironmentVariable("USERNAME"),
            ["PASSWORD"] = Environment.GetEnvironmentVariable("PASSWORD")
        };

        return EnvVars;
    }

    public void GetConnection()
    {
        Console.WriteLine("Setting the connection string");

        Dictionary<string, string?> EnvVars = GetEnvVariables();

        string connectionString = $"Host={EnvVars["HOST"]};database={EnvVars["DATABASE"]};Username={EnvVars["USERNAME"]};Password={EnvVars["PASSWORD"]};";

        var connection = new NpgsqlConnection(connectionString);
        _connection = connection;
    }

}