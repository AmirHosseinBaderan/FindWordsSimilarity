using System.Data;

namespace StringSimilarity;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}