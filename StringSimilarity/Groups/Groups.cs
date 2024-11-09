using Dapper;

namespace StringSimilarity.Groups;

public class Groups(ISqlConnectionFactory sqlConnectionFactory) : IGroups
{
    public Task<IEnumerable<string>> GetGroupsNamesAsync()
    {
        var conn = sqlConnectionFactory.GetOpenConnection();
        const string sql = $"SELECT [CategoryName] FROM [dbo].[{Constants.TableName}]";
        return conn.QueryAsync<string>(sql);
    }
}
