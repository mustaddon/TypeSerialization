using System.Collections.Generic;
using System.Linq;

namespace TypeSerialization._internal;

internal class SerializationFormat(string format, char open, char close, char sep)
{
    public readonly string Format = format;
    public readonly char Open = open;
    public readonly char Close = close;
    public readonly char Sep = sep;
#if NETSTANDARD2_0
    public readonly string SepStr = sep.ToString();
#endif



    public static readonly SerializationFormat[] Values = [
        new("{0}({1})", '(', ')', '-'),
        new("{0}<{1}>", '<', '>', ','),
    ];

    public static readonly HashSet<char> CloseChars = [.. Values.Select(x => x.Close)];
}
