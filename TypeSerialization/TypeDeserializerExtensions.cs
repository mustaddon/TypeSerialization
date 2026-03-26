using System.Linq;
using System.Reflection;

namespace TypeSerialization;

public static class TypeDeserializerExtensions
{
    public static TypeDeserializer RegisterTypesAssignableTo<TBase>(this TypeDeserializer deserializer, params Assembly[] assemblies)
    {
        var type = typeof(TBase);
        return deserializer.Register(assemblies
            .Distinct()
            .SelectMany(x => x.GetTypes())
            .Where(type.IsAssignableFrom));
    }

    public static TypeDeserializer RegisterNotAbstractTypesAssignableTo<TBase>(this TypeDeserializer deserializer, params Assembly[] assemblies)
    {
        var type = typeof(TBase);
        return deserializer.Register(assemblies
            .Distinct()
            .SelectMany(x => x.GetTypes())
            .Where(x => !x.IsAbstract && type.IsAssignableFrom(x)));
    }
}
