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
    public static readonly Type Type = typeof(Type);
    public static readonly Type Attribute = typeof(Attribute);
    public static readonly Type Exception = typeof(Exception);

    public static bool IsStatic(this Type type) => type.IsAbstract && type.IsSealed;
    public static bool IsAttribute(this Type type) => Attribute.IsAssignableFrom(type);
    public static bool IsException(this Type type) => Exception.IsAssignableFrom(type);


    public static readonly Lazy<Type[]> Defaults = new(() => Array.Empty<Type>()
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
