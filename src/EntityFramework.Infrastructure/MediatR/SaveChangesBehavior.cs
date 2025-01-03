﻿using CraftersCloud.Core.Data;
using CraftersCloud.Core.MediatR;
using CraftersCloud.Core.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.EntityFramework.Infrastructure.MediatR;

[UsedImplicitly]
public abstract class SaveChangesBehavior<TDbContext, TRequest, TResponse>(
    TDbContext dbContext,
    IUnitOfWork unitOfWork,
    ILogger<SaveChangesBehavior<TDbContext, TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest where TDbContext : EntitiesDbContext
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = default(TResponse);
        var typeName = request.GetGenericTypeName();

        try
        {
            var (skipSaveChanges, reason) = ShouldSkipSaveChanges(request);
            if (skipSaveChanges)
            {
                logger.LogDebug("Skipping SaveChanges for {CommandName}, Reason: {Reason}", typeName, reason);
                return await next();
            }

            var transactionBehavior = GetTransactionBehavior(request);
            if (transactionBehavior.RequiresTransaction)
            {
                var strategy = dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction =
                        await dbContext.BeginTransactionAsync(transactionBehavior.IsolationLevel, cancellationToken);
                    var transactionId = transaction.TransactionId;
                    using (logger.BeginScope(
                               new List<KeyValuePair<string, object>> { new("TransactionContext", transactionId) }))
                    {
                        logger.LogDebug("Begin transaction {TransactionId} for {CommandName}", transactionId, typeName);

                        response = await next();

                        logger.LogDebug("Commit transaction {TransactionId} for {CommandName}", transactionId,
                            typeName);

                        await unitOfWork.SaveChangesAsync(cancellationToken);
                        await dbContext.CommitTransactionAsync(transaction, cancellationToken);
                    }
                });
            }
            else
            {
                response = await next();

                logger.LogDebug("Saving changes without transaction for {CommandName}", typeName);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return response!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Handling transaction for {CommandName}", typeName);
            throw;
        }
    }

    private static CommandTransactionBehavior GetTransactionBehavior(TRequest request) =>
        request is IBaseCommand command
            ? command.TransactionBehavior
            : CommandTransactionBehavior.NoTransaction;

    private (bool doSkip, string reason) ShouldSkipSaveChanges(TRequest request)
    {
        if (dbContext.HasActiveTransaction)
        {
            return (true, "Transaction is already active");
        }

        return request.IsQuery() ? (true, "Request is a query") : (false, string.Empty);
    }
}