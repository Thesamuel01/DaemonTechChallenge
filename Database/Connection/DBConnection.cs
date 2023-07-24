using MySqlConnector;
using System.Data;

namespace DaemonTechChallenge.Data.Connection;

public class DBConnection : IDBConnection
{
    private MySqlConnection _connection;
    private readonly MySqlConnectionStringBuilder _connectionString;

    public DBConnection(string connectionString)
    {
        _connectionString = new MySqlConnectionStringBuilder(connectionString);
        _connection = new MySqlConnection(_connectionString.ConnectionString); ;
    }

    public async Task<bool> CloseAsync()
    {
        if (_connection.State == ConnectionState.Open)
        {
            await _connection.CloseAsync();
        }

        return _connection.State == ConnectionState.Closed;
    }

    public async Task<bool> OpenAsync()
    {
        if (_connection.State == ConnectionState.Closed)
        {
            _connection = new MySqlConnection(_connectionString.ConnectionString);

            await _connection.OpenAsync();
        }
        
        return _connection.State == ConnectionState.Open;
    }

    public async Task<bool> SaveAsync(DataTable bulk, string tablename)
    {
        if (_connection == null)
        {
            return false;
        }

        var bulkCopy = new MySqlBulkCopy(_connection)
        {
            DestinationTableName = tablename,
        };
        var response = await bulkCopy.WriteToServerAsync(bulk);

        foreach (var warning in response.Warnings)
        {
            Console.WriteLine($"Warning: {warning.Message}");
        }

        return response.RowsInserted > 0;
    }

    public bool Close()
    {
        if (_connection.State == ConnectionState.Open)
        {
            _connection.Close();
        }

        return _connection.State == ConnectionState.Closed;
    }

    public bool Open()
    {
        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }

        return _connection.State == ConnectionState.Open;
    }
}
