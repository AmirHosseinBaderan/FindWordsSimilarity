using Microsoft.Data.SqlClient;
using System.Data;

namespace StringSimilarity;

public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory, IDisposable
{
    private IDbConnection? _connection;

    public void Dispose()
    {
        if (_connection is { State: ConnectionState.Open }) _connection.Dispose();
        GC.SuppressFinalize(this);
    }

    public IDbConnection GetOpenConnection()
    {
        if (_connection is not { State: ConnectionState.Open })
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        return _connection;
    }
}