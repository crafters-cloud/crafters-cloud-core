﻿using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[UsedImplicitly]
public abstract class BaseDbContext(
    EntityRegistrationOptions entityRegistrationOptions,
    DbContextOptions options)
    : DbContext(options)
{
    private IDbContextTransaction? _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;
    public Action<ModelBuilder>? ModelBuilderConfigurator { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(entityRegistrationOptions.ConfigurationAssembly);

        modelBuilder.RegisterEntities(entityRegistrationOptions);

        ModelBuilderConfigurator?.Invoke(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel,
        CancellationToken cancellationToken)
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already active");
        }

        _currentTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        if (transaction != _currentTransaction)
        {
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
        }

        try
        {
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction!.Dispose();
                _currentTransaction = null;
            }
        }
    }

    private void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction!.Dispose();
                _currentTransaction = null;
            }
        }
    }
}