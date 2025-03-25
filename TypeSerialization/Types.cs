using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TypeSerialization;

internal static class Types
{
    internal static readonly Type Type = typeof(Type);
    internal static readonly Type Attribute = typeof(Attribute);
    internal static readonly Type Exception = typeof(Exception);

    internal static bool IsStatic(this Type type) => type.IsAbstract && type.IsSealed;
    internal static bool IsAttribute(this Type type) => Attribute.IsAssignableFrom(type);
    internal static bool IsException(this Type type) => Exception.IsAssignableFrom(type);

    internal static readonly Lazy<Type[]> Defaults = new(() => Array.Empty<Type>()
        .Concat(typeof(int).Assembly.GetTypes().Where(x => typeof(IComparable).IsAssignableFrom(x)))
        .Concat(typeof(List<>).Assembly.GetTypes().Where(x => typeof(IEnumerable).IsAssignableFrom(x)))
        .Where(x => x.IsPublic && !x.IsStatic() && !x.IsEnum)
        .Where(x => !x.IsAttribute())
        .Where(x => !x.IsException())
        .Concat(new[] {
            typeof(object), typeof(Stream),
            typeof(Task), typeof(Task<>), typeof(CancellationToken?)
        }).ToArray());
}
