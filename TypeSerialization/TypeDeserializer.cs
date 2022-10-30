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
            _types = new(() => new TypesCollection(types));
        }

        readonly Lazy<TypesCollection> _types;
        readonly ConcurrentDictionary<string, Type> _deserializedGenerics = new();

        /// <summary>Converts the string representation of a type to an object type.</summary>
        /// <param name="value">A string like: "String", "Array(Int32)", "Dictionary(Int32-String)", ...</param>
        /// <returns>Object type.</returns>
        public Type Deserialize(string value)
        {
            if (value[value.Length - 1] != ')')
                return _types.Value.Simples.TryGetValue(value, out var simple) ? simple
                    : throw TypeNotFoundException(value);

            if (_deserializedGenerics.TryGetValue(value, out var type))
                return type;

            _deserializedGenerics.TryAdd(value, type = Parse(value));

            return type;
        }

        /// <summary>Converts the string representation of types to an array of object types.</summary>
        /// <param name="value">A string like: "Boolean-List(String)-Array(Nullable(Int32))".</param>
        /// <returns>Array of object types.</returns>
        public Type[] DeserializeMany(string value)
        {
            return ExtractTypeStrings(value, 0, value.Length).Select(Deserialize).ToArray();
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
            else if (_types.Value.Generics.TryGetValue($"{typeName}`{parts.Count - 1}", out type) && parts.Skip(1).Any(x => x.Length > 0))
                type = type.MakeGenericType(parts.Skip(1).Select(x => Parse(x)).ToArray());

            return type ?? throw TypeNotFoundException(str);
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

            foreach (var x in ExtractTypeStrings(str, typeLen + 1, str.Length - 1))
                result.Add(x);

            return result;
        }

        static IEnumerable<string> ExtractTypeStrings(string str, int start, int end)
        {
            var openCount = 0;

            for (var i = start; i < end; i++)
            {
                if (str[i] == '-' && openCount <= 0)
                {
                    yield return str.Substring(start, i - start);
                    openCount = 0;
                    start = i + 1;
                }
                else if (str[i] == '(')
                    openCount++;
                else if (str[i] == ')')
                    openCount--;
            }

            yield return str.Substring(start, end - start);
        }

        static KeyNotFoundException TypeNotFoundException(string type) => new($"Type '{type}' not found");
    }

    internal class TypesCollection
    {
        public TypesCollection(IEnumerable<Type> types)
        {
            foreach (var type in types)
                Add(type);
        }

        public Dictionary<string, Type> Simples { get; } = new();
        public Dictionary<string, Type> Generics { get; } = new();

        private void Add(Type type)
        {
            if (!type.IsGenericType)
            {
                var simple = type.IsArray ? type.GetElementType()! : type;

                if (!this.Simples.ContainsKey(simple.Name))
                    this.Simples.Add(simple.Name, simple);

                return;
            }

            foreach (var x in type.GenericTypeArguments)
                Add(x);

            var generic = type.GetGenericTypeDefinition();

            if (!this.Generics.ContainsKey(generic.Name))
                this.Generics.Add(generic.Name, generic);
        }
    }
}
