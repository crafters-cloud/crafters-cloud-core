using System.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;

namespace CraftersCloud.Core.Tests.Shared.Database;

[PublicAPI]
public static class DatabaseInitializer
{
    public static async Task RecreateDatabaseAsync(DbContext dbContext, DatabaseInitializerOptions options)
    {
        if (HasPendingMigrations(dbContext))
        {
            RecreateDatabase(dbContext);
        }
        else
        {
            await ResetDataAsync(dbContext, options.ResetDataOptions);
        }
    }

    private static bool HasPendingMigrations(DbContext dbContext) => dbContext.Database.GetPendingMigrations().Any();

    private static void RecreateDatabase(DbContext dbContext)
    {
        DropAllDbObjects(dbContext);
        dbContext.Database.Migrate();
    }

    private static async Task ResetDataAsync(DbContext dbContext, ResetDataOptions options)
    {
        await DeleteDataAsync(dbContext, options);
        RunCustomQuery(dbContext, options.CustomSqlQuery);
    }

    private static async Task DeleteDataAsync(DbContext dbContext, ResetDataOptions options)
    {
        var dbConnection = dbContext.Database.GetDbConnection();
        if (dbConnection.State != ConnectionState.Open)
        {
            await dbConnection.OpenAsync();   
        }

        var respawner = await Respawner.CreateAsync(dbConnection,
            new RespawnerOptions
            {
                TablesToIgnore = options.TablesToIgnore.Select(name => new Table(name)).ToArray(),
                WithReseed = options.ReseedIdentityColumns
            });

        await respawner.ResetAsync(dbConnection);
    }

    private static void RunCustomQuery(DbContext dbContext, string customSqlQuery)
    {
        if (!string.IsNullOrEmpty(customSqlQuery))
        {
            dbContext.Database.ExecuteSqlRaw(customSqlQuery);
        }
    }

    private static void DropAllDbObjects(DbContext dbContext)
    {
        string dropAllSql;
        if (dbContext.IsSqlServer())
        {
            dropAllSql = DatabaseHelpers.DropAllSqlServerScript;    
        }
        else if (dbContext.IsPostgres())
        {
            dropAllSql = DatabaseHelpers.DropAllPostgreSqlScript;    
        }
        else
        {
            throw new NotSupportedException($"Database type {dbContext.Database.ProviderName} is not supported.");
        }
        
        foreach (var statement in dropAllSql.SplitStatements())
        {
            dbContext.Database.ExecuteSqlRaw(statement);
        }
    }

    private static void WriteLine(string message) => TestContext.WriteLine(message);
    
    
}