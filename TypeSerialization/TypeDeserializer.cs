using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TypeSerialization._internal;

namespace TypeSerialization;

public sealed class TypeDeserializer(IEnumerable<(string, Type)>? types = null)
{
    public TypeDeserializer(IEnumerable<Type> types)
        : this(types?.Select(x => (Types.NameGetter(x), x)))
    { }

    readonly Lazy<TypesCollection> _types = new(() => new TypesCollection(Types.Defaults.Value.Concat(types ?? []).Select(x => (x.Item2, x.Item1))));
    readonly ConcurrentDictionary<string, Lazy<Type>> _deserializedGenerics = [];

    public static TypeDeserializer Default => _default.Value;
    static readonly Lazy<TypeDeserializer> _default = new(() => new());

    /// <summary>Converts the string representation of a type to an object type.</summary>
    /// <param name="value">A string like: "String", "Array(Int32)", "Dictionary(Int32-String)", ...</param>
    /// <returns>Object type.</returns>
    public Type? Deserialize(string? value)
    {
        if (value == null || value.Length == 0)
            return null;

        var lastChar = value[value.Length - 1];

        if (!SerializationFormat.CloseChars.Contains(lastChar))
        {
            if (TryNormFormat(SerializationFormat.Values[0], value, out var formatted))
                return Deserialize(formatted);

            return _types.Value.Simples.TryGetValue(value, out var simple) ? simple
                : throw NotRegisteredException(value);
        }

        if (value[0] == ' ')
            value = value.TrimStart();

        return _deserializedGenerics.GetOrAdd(value,
            k => new(() => Parse(SerializationFormat.Values.First(x => x.Close == lastChar), value)))
            .Value;
    }


    /// <summary>Converts the string representation of types to an array of object types.</summary>
    /// <param name="value">A string like: "Boolean-List(String)-Array(Nullable(Int32))".</param>
    /// <returns>Array of object types.</returns>
    public Type?[] DeserializeMany(string? value)
    {
        if (value == null || value.Length == 0)
            return [];

        var format = SerializationFormat.Values.FirstOrDefault(x => value.Contains(x.Sep));

        if (format == null)
            return [Deserialize(value)];

        return [.. ExtractTypeStrings(format, value, 0, value.Length).Select(Deserialize)];
    }

    public TypeDeserializer Register(params Type[] types) => Register(types.AsEnumerable());
    public TypeDeserializer Register(IEnumerable<Type> types) => Register(types.Select(t => (Types.NameGetter(t), t)));

    public TypeDeserializer Register(params (string Name, Type Type)[] types) => Register(types.AsEnumerable());
    public TypeDeserializer Register(IEnumerable<(string Name, Type Type)> types)
    {
        foreach (var (key, type) in types)
            _types.Value.Add(type, key);

        return this;
    }

    static bool TryNormFormat(SerializationFormat format, string str, out string formatted)
    {
        formatted = default!;
        var result = false;
        var lastChar = str[str.Length - 1];

        if (lastChar == ' ' || str[0] == ' ')
        {
            formatted = str = str.Trim();
            lastChar = str[str.Length - 1];
            result = true;
        }

        if (lastChar == '?')
        {
            formatted = string.Format(format.Format, nameof(Nullable), str.Substring(0, str.Length - 1));
            result = true;
        }
        else if (lastChar == ']')
        {
            formatted = string.Format(format.Format, nameof(Array), str.Substring(0, str.Length - 2));
            result = true;
        }

        return result;
    }

    Type ParseWithNorm(SerializationFormat format, string str)
    {
        return Parse(format, TryNormFormat(format, str, out var formatted) ? formatted : str);
    }

    Type Parse(SerializationFormat format, string str)
    {
        var parts = TypeParts(format, str);
        var typeName = parts.First();

        if (parts.Count == 2 && typeName == nameof(Array))
            return Array.CreateInstance(ParseWithNorm(format, parts.Last()), 0).GetType();

        Type? type = null;

        if (parts.Count == 1)
            _types.Value.Simples.TryGetValue(typeName, out type);
        else if (_types.Value.Generics.TryGetValue($"{typeName}`{parts.Count - 1}", out type) && parts.Skip(1).Any(x => x.Length > 0))
            type = type.MakeGenericType(parts.Skip(1).Select(x => ParseWithNorm(format, x)).ToArray());

        if (type != null)
            return type;

        if (parts.Count < 2)
            throw NotRegisteredException(typeName);

        throw NotRegisteredException(string.Format("{0}<{1}>", typeName, new string(',', parts.Count - 2)));
    }

    static List<string> TypeParts(SerializationFormat format, string str)
    {
        var result = new List<string>();
        var typeLen = str.IndexOf(format.Open);

        if (typeLen < 0)
        {
            result.Add(str);
            return result;
        }

        result.Add(str.Substring(0, typeLen));

        foreach (var x in ExtractTypeStrings(format, str, typeLen + 1, str.Length - 1))
            result.Add(x);

        return result;
    }

    static IEnumerable<string> ExtractTypeStrings(SerializationFormat format, string str, int start, int end)
    {
        var openCount = 0;

        for (var i = start; i < end; i++)
        {
            if (str[i] == format.Sep && openCount <= 0)
            {
                yield return str.Substring(start, i - start);
                openCount = 0;
                start = i + 1;
            }
            else if (str[i] == format.Open)
                openCount++;
            else if (str[i] == format.Close)
                openCount--;
        }

        yield return str.Substring(start, end - start);
    }

    static KeyNotFoundException NotRegisteredException(string type) => new($"Type '{type}' not registered");
}

internal class TypesCollection
{
    public TypesCollection(IEnumerable<(Type, string)> types)
    {
        foreach (var (type, k) in types)
            Add(type, k);
    }

    public Dictionary<string, Type> Simples { get; } = [];
    public Dictionary<string, Type> Generics { get; } = [];

    public void Add(Type type, string key)
    {
        if (!type.IsGenericType)
        {
            var simple = type.IsArray ? type.GetElementType()! : type;
            this.Simples[key] = simple;
            return;
        }

        foreach (var x in type.GenericTypeArguments)
            Add(x, Types.NameGetter(x));

        var generic = type.GetGenericTypeDefinition();
        this.Generics[key] = generic;
    }
}
