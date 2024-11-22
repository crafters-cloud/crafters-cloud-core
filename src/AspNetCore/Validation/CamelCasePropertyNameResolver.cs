using System.Linq.Expressions;
using System.Reflection;
using CraftersCloud.Core.Helpers;
using FluentValidation.Internal;
using JetBrains.Annotations;

namespace CraftersCloud.Core.AspNetCore.Validation;

// Enables FluentValidation error messages to contain camel cased property names instead of pascal cased.
// E.g. Instead of 'UserName' we get 'userName'
[PublicAPI]
public static class CamelCasePropertyNameResolver
{
    public static string ResolvePropertyName(Type _, MemberInfo memberInfo, LambdaExpression? expression)
    {
        var propertyName = DefaultPropertyNameResolver(memberInfo, expression);

        return propertyName.ToCamelCase();
    }

    private static string DefaultPropertyNameResolver(MemberInfo memberInfo, LambdaExpression? expression)
    {
        if (expression != null)
        {
            var chain = PropertyChain.FromExpression(expression);
            if (chain.Count > 0)
            {
                return chain.ToString();
            }
        }

        return memberInfo.Name;
    }
}