using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TypeSerialization
{
    public sealed class TypeDeserializer
    {
        public TypeDeserializer(IEnumerable<Type> types)
        {
            _types = new Lazy<TypesCollection>(() => InitTypesCollection(types));
        }

        readonly Lazy<TypesCollection> _types;
        readonly ConcurrentDictionary<string, Type> _deserializedGenerics = new();

        /// <summary>Converts the string representation of a type to an object type.</summary>
        /// <param name="value">A string like: "String", "Array(Int32)", "Dictionary(Int32-String)", ...</param>
        /// <returns>An object type.</returns>
        public Type Deserialize(string value)
        {
            if (_types.Value.Simples.TryGetValue(value, out var type))
                return type;

            if (_deserializedGenerics.TryGetValue(value, out type))
                return type;

            _deserializedGenerics.TryAdd(value, type = Parse(value));

            return type;
        }

        Type Parse(string str)
        {
            var parts = TypeParts(str);
            var typeName = parts.First();

            if (parts.Count == 2 && typeName == nameof(Array))
                return Array.CreateInstance(Parse(parts.Last()), 0).GetType();

            Type? type = null;

            if (parts.Count == 1)
                _types.Value.Simples.TryGetValue(typeName, out type);
            else if (_types.Value.Generics.TryGetValue($"{typeName}`{parts.Count - 1}", out type))
                type = type.MakeGenericType(parts.Skip(1).Select(x => Parse(x)).ToArray());

            return type ?? throw new KeyNotFoundException($"Type '{str}' not found");
        }

        static List<string> TypeParts(string str)
        {
            var result = new List<string>();
            var typeLen = str.IndexOf('(');

            if (typeLen < 0)
            {
                result.Add(str);
                return result;
            }

            result.Add(str.Substring(0, typeLen));

            var openCount = 0;
            var start = typeLen + 1;
            var end = str.Length - 1;

            for (var i = start; i <= end; i++)
            {
                if (i == end || (str[i] == '-' && openCount <= 0))
                {
                    result.Add(str.Substring(start, i - start));
                    openCount = 0;
                    start = i + 1;
                }
                else if (str[i] == '(')
                    openCount++;
                else if (str[i] == ')')
                    openCount--;
            }

            return result;
        }

        static TypesCollection InitTypesCollection(IEnumerable<Type> types)
        {
            var result = new TypesCollection();

            foreach (var type in types)
            {
                var dict = type.IsGenericType ? result.Generics
                    : result.Simples;

                if (!dict.ContainsKey(type.Name))
                    dict.Add(type.Name, type);
            }

            return result;
        }

        class TypesCollection
        {
            public Dictionary<string, Type> Simples { get; } = new();
            public Dictionary<string, Type> Generics { get; } = new();
        }
    }
}
