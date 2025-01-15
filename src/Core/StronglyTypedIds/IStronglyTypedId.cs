namespace CraftersCloud.Core.StronglyTypedIds;

public interface IStronglyTypedId<out TValue> where TValue : struct
{
    TValue Value { get; }
}