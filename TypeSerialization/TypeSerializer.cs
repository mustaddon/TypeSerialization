using System;
using System.Collections.Generic;
using System.Linq;
using TypeSerialization._internal;

namespace TypeSerialization;

public static class TypeSerializer
{
    /// <summary>Converts an object type to a string representation.</summary>
    /// <param name="type">An object type.</param>
    /// <returns>A string like: "String", "Array(Int32)", "Dictionary(Int32-String)", ...</returns>
    public static string Serialize(this Type type, Formats format = Formats.UriSafe, Func<Type, string>? nameGetter = null)
    {
        nameGetter ??= Types.NameGetter;

        if (type.IsArray)
            return string.Format(
                SerializationFormat.Values[(int)format].Format,
                nameGetter(Types.Array),
                Serialize(type.GetElementType()!, format, nameGetter));

        if (!type.IsPublic && Types.Type.IsAssignableFrom(type))
            return nameGetter(Types.Type);

        if (!type.IsGenericType)
            return nameGetter(type);

        var name = nameGetter(type);
        var nameLength = name.IndexOf('`');

        if (nameLength >= 0)
            name = name.Substring(0, nameLength);

        return string.Format(
            SerializationFormat.Values[(int)format].Format,
            name, Serialize(type.GetGenericArguments(), format, nameGetter));
    }

    /// <summary>Converts an array of object types to a string representation.</summary>
    /// <param name="types">Object types.</param>
    /// <returns>A string like: "Boolean-List(String)-Array(Nullable(Int32))".</returns>
    public static string Serialize(this IEnumerable<Type?> types, Formats format = Formats.UriSafe, Func<Type, string>? nameGetter = null)
    {
        return string.Join(
#if NET6_0_OR_GREATER
            SerializationFormat.Values[(int)format].Sep,
#else
            SerializationFormat.Values[(int)format].SepStr,
#endif
            types.Select(x => x == null || x.IsGenericParameter ? string.Empty : Serialize(x, format, nameGetter)));
    }
}
