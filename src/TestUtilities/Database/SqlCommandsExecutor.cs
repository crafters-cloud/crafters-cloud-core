

using Microsoft.Data.SqlClient;

namespace CraftersCloud.Core.TestUtilities.Database;

public static class SqlCommandsExecutor
{
    public static void ExecuteSqlCommand(SqlConnectionStringBuilder builder, string queryText)
    {
        ExecuteSqlCommand(builder.ConnectionString, queryText);
    }

    public static List<T> ExecuteSqlQuery<T>(SqlConnectionStringBuilder connectionStringBuilder, string queryText,
        Func<SqlDataReader, T> readFunc)
    {
        return ExecuteSqlQuery(connectionStringBuilder.ConnectionString, queryText, readFunc);
    }

    public static void ExecuteSqlCommand(string connectionString, string queryText)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = queryText;
        command.ExecuteNonQuery();
    }

    public static List<T> ExecuteSqlQuery<T>(string connectionString, string queryText,
        Func<SqlDataReader, T> readFunc)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var result = new List<T>();
        using var command = connection.CreateCommand();
        command.CommandText = queryText;
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(readFunc(reader));
        }
        return result;
    }
}