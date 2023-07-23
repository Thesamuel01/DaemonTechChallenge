using MySqlConnector;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DaemonTechChallenge.Helpers;

public class DBConnection : IDBConnection
{
    private MySqlConnection? _connection;
    private readonly MySqlConnectionStringBuilder _connectionString;

    public DBConnection(string connectionString)
    {
        _connectionString = new MySqlConnectionStringBuilder(connectionString);
        _connection = null;
    }

    public async Task<bool> CloseAsync()
    {
        if (_connection == null)
        {
            return false;
        }

        await _connection.CloseAsync();

        return _connection.State == ConnectionState.Closed;
    }

    public async Task<bool> OpenAsync()
    {
        if (_connection == null)
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
        if (_connection == null)
        {
            return false;
        }

        _connection.Close();

        return _connection.State == ConnectionState.Closed;
    }

    public bool Open()
    {
        if (_connection == null)
        {
            _connection = new MySqlConnection(_connectionString.ConnectionString);

            _connection.Open();
        }

        return _connection.State == ConnectionState.Open;
    }
}
