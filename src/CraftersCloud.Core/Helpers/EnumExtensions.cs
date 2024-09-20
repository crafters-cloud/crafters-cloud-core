﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JetBrains.Annotations;

namespace CraftersCloud.Core.Helpers
{
    [PublicAPI]
    public static class EnumExtensions
    {
        public static string GetDisplayName(this System.Enum value)
        {
            var attribute = value.GetAttribute<DisplayAttribute>();

            return attribute?.Name ?? string.Empty;
        }

        public static string GetDescription(this System.Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();

            return attribute?.Description ?? string.Empty;
        }

        public static T? GetAttribute<T>(this System.Enum value)
            where T : Attribute
        {
            var type = value.GetType();
            var field = type.GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<T>();

            return attribute;
        }
    }
}