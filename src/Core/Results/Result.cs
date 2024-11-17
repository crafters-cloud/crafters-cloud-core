namespace CraftersCloud.Core.Results;

public static class Result
{
    public static NotFoundResult NotFound() => new();

    public static NoContentResult NoContent() => new();

    public static CreatedResult<T> Created<T>(T value) => new(value);
}