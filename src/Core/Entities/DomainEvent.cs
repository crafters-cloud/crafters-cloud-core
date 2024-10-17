using MediatR;

namespace CraftersCloud.Core.Entities;

public abstract record DomainEvent : INotification;