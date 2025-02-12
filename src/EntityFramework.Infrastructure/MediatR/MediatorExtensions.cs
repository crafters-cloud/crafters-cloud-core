﻿using System.Diagnostics;
using CraftersCloud.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.EntityFramework.Infrastructure.MediatR;

internal static class MediatorExtensions
{
    public static IEnumerable<DomainEvent> GatherDomainEventsFromContext(this DbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.GetDomainEvents().Any()).ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.GetDomainEvents())
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        return domainEvents;
    }

    public static async Task PublishDomainEventsAsync(this IMediator mediator, IEnumerable<DomainEvent> domainEvents,
        ILogger logger, CancellationToken cancellationToken = default)
    {
        var stopWatch = Stopwatch.StartNew();
        // sequentially publish domain events to avoid problems with same DbContext used by different threads 
        // fixes problem "A second operation started on this context before a previous operation completed"
        // this happens when one event handler is doing DbContext saving while some other one is doing the reading
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
            var ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            logger.LogDebug("Time to publish domain event - {DomainEvent}: {Time}s", domainEvent.GetType(),
                ts.TotalSeconds);
            stopWatch.Restart();
        }
    }
}