using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeSerialization
{
    public static class TypeSerializer
    {
        /// <summary>Converts an object type to a string representation.</summary>
        /// <param name="type">An object type.</param>
        /// <returns>A string like: "String", "Array(Int32)", "Dictionary(Int32-String)", ...</returns>
        public static string Serialize(this Type type)
        {
            if (type.IsArray)
                return $"{nameof(Array)}({Serialize(type.GetElementType()!)})";

            if (!type.IsPublic && Types.Type.IsAssignableFrom(type))
                return nameof(Type);

            if (!type.IsGenericType)
                return type.Name;

            return $"{type.Name.Split('`').First()}({Serialize(type.GetGenericArguments())})";
        }

        /// <summary>Converts an array of object types to a string representation.</summary>
        /// <param name="types">Object types.</param>
        /// <returns>A string like: "Boolean-List(String)-Array(Nullable(Int32))".</returns>
        public static string Serialize(this IEnumerable<Type?> types)
        {
            return string.Join("-", types.Select(x => x == null || x.IsGenericParameter ? string.Empty : Serialize(x)));
        }
    }
}
