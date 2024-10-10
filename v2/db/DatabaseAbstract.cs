using Npgsql;

namespace Db;

public abstract class DatabaseAbstract {
    public NpgsqlConnection? _connection;

    public void GetConnection()
     {  
        Console.WriteLine("Setting the connection string");
        //todo move this to env file
        string connectionString = "Host=localhost;Database=hiroji;Username=hiroji;Password=password;";
        
        var connection = new NpgsqlConnection(connectionString);
        _connection = connection;
    }

}