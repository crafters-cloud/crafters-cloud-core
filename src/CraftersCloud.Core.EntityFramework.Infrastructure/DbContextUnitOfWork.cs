using CraftersCloud.Core.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[UsedImplicitly]
public class DbContextUnitOfWork(DbContext context, ILogger<DbContextUnitOfWork> logger) : IUnitOfWork
{
    private bool _cancelSaving;

    public int SaveChanges()
    {
        if (_cancelSaving)
        {
            logger.LogWarning("Not saving database changes since saving was cancelled.");
            return 0;
        }

        var numberOfChanges = context.SaveChanges();
        logger.LogDebug(
            "{NumberOfChanges} of changed were saved to database {Database}", numberOfChanges, context.Database.GetDbConnection().Database);

        return numberOfChanges;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_cancelSaving)
        {
            logger.LogWarning("Not saving database changes since saving was cancelled.");
            return 0;
        }

        var numberOfChanges = await context.SaveChangesAsync(cancellationToken);
        logger.LogDebug(
            "{NumberOfChanges} of changed were saved to database {Database}", numberOfChanges, context.Database.GetDbConnection().Database);
        return numberOfChanges;
    }

    public void CancelSaving() => _cancelSaving = true;
}