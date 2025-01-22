namespace CraftersCloud.Core.StronglyTypedIds;

/// Represents metadata about a strongly-typed ID, including the strongly-typed ID's type and its underlying value type.
/// This object is primarily used to associate a specific strongly-typed ID implementation with the primitive data type
/// that serves as its underlying value.
internal record StronglyTypedIdMetaData(Type Type, Type ValueType);