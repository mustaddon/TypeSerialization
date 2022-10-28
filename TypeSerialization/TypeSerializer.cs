using System;
using System.Linq;

namespace TypeSerialization
{
    public static class TypeSerializer
    {
        /// <summary>Converts an object type to a string representation.</summary>
        /// <param name="type">An object type</param>
        /// <returns>A string like: "String", "Array(Int32)", "Dictionary(Int32-String)", ...</returns>
        public static string Serialize(this Type type)
        {
            if (type.IsArray)
                return $"{nameof(Array)}({Serialize(type.GetElementType()!)})";

            if (!type.IsGenericType)
                return type.Name;

            var genericArgs = type.GetGenericArguments().Select(x => x.IsGenericParameter ? string.Empty : Serialize(x));

            return $"{type.Name.Split('`').First()}({string.Join("-", genericArgs)})";
        }
    }
}
