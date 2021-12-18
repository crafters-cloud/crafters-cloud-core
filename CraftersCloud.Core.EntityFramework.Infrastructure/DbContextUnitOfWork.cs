using CraftersCloud.Core.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[UsedImplicitly]
public class DbContextUnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly ILogger<DbContextUnitOfWork> _logger;
    private bool _cancelSaving;

    public DbContextUnitOfWork(DbContext context, ILogger<DbContextUnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void SaveChanges()
    {
        if (_cancelSaving)
        {
            _logger.LogWarning("Not saving database changes since saving was cancelled.");
            return;
        }

        var numberOfChanges = _context.SaveChanges();
        _logger.LogDebug(
            $"{numberOfChanges} of changed were saved to database {_context.Database.GetDbConnection().Database}");
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_cancelSaving)
        {
            _logger.LogWarning("Not saving database changes since saving was cancelled.");
            return;
        }

        var numberOfChanges = await _context.SaveChangesAsync(cancellationToken);
        _logger.LogDebug(
            $"{numberOfChanges} of changed were saved to database {_context.Database.GetDbConnection().Database}");
    }

    public void CancelSaving() => _cancelSaving = true;
}