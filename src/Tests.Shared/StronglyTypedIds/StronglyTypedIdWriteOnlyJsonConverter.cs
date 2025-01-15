using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.Core.Tests.Shared.StronglyTypedIds;

internal class StronglyTypedIdWriteOnlyJsonConverter<T, TId> : WriteOnlyJsonConverter<T>
    where T : IStronglyTypedId<TId>
    where TId : struct
{
    public override void Write(VerifyJsonWriter writer, T value) => writer.WriteValue(value.Value);
}