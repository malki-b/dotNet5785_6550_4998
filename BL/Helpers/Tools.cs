using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

 internal static class Tools
{
    /// <summary>
    /// Converts the properties of an object to a string representation.
    /// </summary>
    /// <returns>A string representation of the object's properties.</returns>
    public static string ToStringProperty<T>(this T t)
    {
        if (t == null) return "null";

        var properties = typeof(T).GetProperties();
        var stringBuilder = new System.Text.StringBuilder();

        foreach (var property in properties)
        {
            var value = property.GetValue(t);
            stringBuilder.AppendLine($"{property.Name}: {value}");
        }

        return stringBuilder.ToString();
    }

}
