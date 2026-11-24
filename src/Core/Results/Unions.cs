namespace CraftersCloud.Core.Results;

/// <summary>
/// The result of a query that looks up a single item: the item, or <see cref="NotFoundResult"/>.
/// </summary>
[PublicAPI]
public union QueryResult<T>(T, NotFoundResult);

/// <summary>
/// The result of a command that creates a resource.
/// </summary>
[PublicAPI]
public union CreateResult<T>(CreatedResult<T>, BadRequestResult);

/// <summary>
/// The result of a command that updates or deletes a resource.
/// </summary>
[PublicAPI]
public union UpdateResult(NoContentResult, NotFoundResult, BadRequestResult);

/// <summary>
/// The result of a command with its own failures. <typeparamref name="TError"/> is typically a union of
/// failure records of the use case, e.g. <c>union ReserveBookingError(ApartmentNotFound, Overlap)</c>.
/// A failure has to be cast to <typeparamref name="TError"/> before it converts to this union:
/// <c>return (ReserveBookingError)new Overlap(...);</c>
/// </summary>
[PublicAPI]
public union CommandResult<T, TError>(T, TError, BadRequestResult);
