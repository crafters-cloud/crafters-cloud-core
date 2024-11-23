using Microsoft.Data.SqlClient;

namespace CraftersCloud.Core.TestUtilities.Database;

public static class DatabaseCreator
{
    private static readonly string IntegrationTestsDbName = "IntegrationTestsDb";
    private static readonly string DevelopmentDbName = "WorkBench";

    private static SqlConnectionStringBuilder Master =>
        new SqlConnectionStringBuilder
        {
            DataSource = @"(LocalDb)\MSSQLLocalDB",
            TrustServerCertificate = true, 
            IntegratedSecurity = true,
            InitialCatalog = "master"
        };

    public static void RecreateTestDatabase()
    {
        Recreate(IntegrationTestsDbName);
    }

    public static void DestroyTestDatabase()
    {
        Destroy(IntegrationTestsDbName);
    }

    public static void RecreateDevelopmentDatabase()
    {
        Recreate(DevelopmentDbName);
    }

    private static void Recreate(string dbName)
    {
        Destroy(dbName);
        SqlCommandsExecutor.ExecuteSqlCommand(Master, $"CREATE DATABASE [{dbName}]");
    }

    private static void Destroy(string dbName)
    {
        List<object> database = SqlCommandsExecutor.ExecuteSqlQuery(Master, $@"SELECT name FROM master.dbo.sysdatabases WHERE name = '{dbName}'", row => row["name"]);
        if (database.Count == 0)
        {
            return;
        }
        SqlCommandsExecutor.ExecuteSqlCommand(Master, $"ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{dbName}]");
    }
}