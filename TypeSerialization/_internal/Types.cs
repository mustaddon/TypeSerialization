using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace TypeSerialization._internal;

internal static class Types
{
    internal static string NameGetter(Type type) => type.Name;

    internal static readonly Lazy<(string Name, Type Type)[]> Defaults = new(()
        =>
        [
            .. BuiltTypes(),
            .. DefaultDataTypes().Select(type => (NameGetter(type), type)),
        ]);

    internal static IEnumerable<(string Name, Type Type)> BuiltTypes()
    {
        yield return ("object", typeof(object));
        yield return ("string", typeof(string));
        yield return ("bool", typeof(bool));
        yield return ("char", typeof(char));
        yield return ("byte", typeof(byte));
        yield return ("sbyte", typeof(sbyte));
        yield return ("short", typeof(short));
        yield return ("ushort", typeof(ushort));
        yield return ("int", typeof(int));
        yield return ("uint", typeof(uint));
        yield return ("nint", typeof(nint));
        yield return ("nuint", typeof(nuint));
        yield return ("long", typeof(long));
        yield return ("ulong", typeof(ulong));
        yield return ("float", typeof(float));
        yield return ("double", typeof(double));
        yield return ("decimal", typeof(decimal));
    }

    internal static IEnumerable<Type> DefaultDataTypes()
    {
        foreach (var (_, type) in BuiltTypes())
            yield return type;

        yield return typeof(void);
        yield return typeof(Type);
        yield return typeof(Nullable<>);
        yield return typeof(CancellationToken);
        yield return typeof(DateTime);
        yield return typeof(DateTimeOffset);
        yield return typeof(DateTimeKind);
        yield return typeof(TimeSpan);
        yield return typeof(Guid);
        yield return typeof(KeyValuePair<,>);
        yield return typeof(Version);
        yield return typeof(Task);
        yield return typeof(Task<>);
        yield return typeof(Lazy<>);
        yield return typeof(Stream);
        yield return typeof(MemoryStream);
        yield return typeof(FileStream);
        yield return typeof(Array);
        yield return typeof(Hashtable);
        yield return typeof(IEnumerable);
        yield return typeof(IEnumerable<>);
        yield return typeof(List<>);
        yield return typeof(IList);
        yield return typeof(IList<>);
        yield return typeof(IReadOnlyList<>);
        yield return typeof(Collection<>);
        yield return typeof(ICollection);
        yield return typeof(ICollection<>);
        yield return typeof(HashSet<>);
        yield return typeof(ISet<>);
        yield return typeof(Dictionary<,>);
        yield return typeof(ConcurrentDictionary<,>);
        yield return typeof(IDictionary);
        yield return typeof(IDictionary<,>);
        yield return typeof(IReadOnlyDictionary<,>);
        yield return typeof(ConcurrentBag<>);
        yield return typeof(IProducerConsumerCollection<>);
        yield return typeof(Queue<>);
        yield return typeof(ConcurrentQueue<>);
        yield return typeof(Stack<>);
        yield return typeof(ConcurrentStack<>);

#if NET6_0_OR_GREATER
        yield return typeof(Half);
        yield return typeof(NFloat);
        yield return typeof(TimeOnly);
        yield return typeof(DateOnly);
        yield return typeof(IReadOnlySet<>);
        yield return typeof(ValueTask);
        yield return typeof(ValueTask<>);
        yield return typeof(IAsyncEnumerable<>);
#endif
#if NET7_0_OR_GREATER
        yield return typeof(Int128);
        yield return typeof(UInt128);
#endif

        yield return typeof(Tuple<>);
        yield return typeof(Tuple<,>);
        yield return typeof(Tuple<,,>);
        yield return typeof(Tuple<,,,>);
        yield return typeof(Tuple<,,,,>);
        yield return typeof(Tuple<,,,,,>);
        yield return typeof(Tuple<,,,,,,>);
        yield return typeof(Tuple<,,,,,,,>);

        yield return typeof(ValueTuple<>);
        yield return typeof(ValueTuple<,>);
        yield return typeof(ValueTuple<,,>);
        yield return typeof(ValueTuple<,,,>);
        yield return typeof(ValueTuple<,,,,>);
        yield return typeof(ValueTuple<,,,,,>);
        yield return typeof(ValueTuple<,,,,,,>);
        yield return typeof(ValueTuple<,,,,,,,>);
    }
}
