using System.Reflection;
using CraftersCloud.Core.Helpers;
using OneOf;

namespace CraftersCloud.Core.Results;

public static class TypeExtensions
{
    public static bool IsDerivedFromOneOfType<T>(this Type type) =>
        type.FindAllBaseTypes().Any(baseType =>
            baseType.ImplementsInterface(typeof(IOneOf)) &&
            Array.Exists(baseType.GetGenericArguments(), g => g == typeof(T)));

    private static IEnumerable<Type> FindAllBaseTypes(this Type type)
    {
        var baseType = type.BaseType;
        while (baseType != null && baseType != typeof(object))
        {
            yield return baseType;
            baseType = baseType.BaseType;
        }
    }

    public static TResult MapToOneOf<TResult>(this IResult value) 
    {
        var targetType = typeof(TResult);
        var valueType = value.GetType();
        
        // find the constructor that accepts an IOneOf parameter
        var constructor = targetType.GetConstructors()
                              .FirstOrDefault(ctor => ctor.GetParameters()
                                  .Any(p => typeof(IOneOf).IsAssignableFrom(p.ParameterType)))
                          ?? throw new InvalidOperationException(
                              $"No constructor found in {targetType} that accepts an IOneOf parameter.");

        // find the implicit conversion operator that converts Value to the parameter type
        var parameterType = constructor.GetParameters()[0].ParameterType;
        var method = parameterType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                         .FirstOrDefault(m => m.Name == "op_Implicit" &&
                                              m.GetParameters().Any(p => p.ParameterType == valueType))
                     ?? throw new InvalidOperationException($"No implicit conversion operator found. Maybe '{targetType.Name}' class does not inherits from OneOfBase with '{valueType.Name}'.");

        var convertedType = method.Invoke(null, [value]) ?? value;
        return (TResult) constructor.Invoke([convertedType]);
    }
}