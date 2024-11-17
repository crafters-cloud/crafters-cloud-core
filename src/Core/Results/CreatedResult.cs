using Ardalis.Result;

namespace CraftersCloud.Core.Results;

public class CreatedResult<T> : Result<T>
{
    public CreatedResult(T value) : base(ResultStatus.Created) => Value = value;
}