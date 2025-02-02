using Npgsql;
using v2.utils;

namespace v2.Db;

public abstract class DatabaseAbstract
{
    public NpgsqlConnection? Connection;
    public DatabaseCommands Commands = new DatabaseCommands();

    private static Dictionary<string, string?> GetEnvVariables()
    {
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, ".env");
        DotEnv.Load(dotenv);

        var envVars = new Dictionary<string, string?>
        {
            ["HOST"] = Environment.GetEnvironmentVariable("HOST"),
            ["DATABASE"] = Environment.GetEnvironmentVariable("DATABASE"),
            ["USERNAME"] = Environment.GetEnvironmentVariable("USERNAME"),
            ["PASSWORD"] = Environment.GetEnvironmentVariable("PASSWORD")
        };

        return envVars;
    }

    public NpgsqlConnection GetIndividualConnection()
    {
        Console.WriteLine("Setting the connection string");

        Dictionary<string, string?> envVars = GetEnvVariables();

        string connectionString = $"Host={envVars["HOST"]};database={envVars["DATABASE"]};Username={envVars["USERNAME"]};Password={envVars["PASSWORD"]};";

        var connection = new NpgsqlConnection(connectionString);
        return connection;
    }

}