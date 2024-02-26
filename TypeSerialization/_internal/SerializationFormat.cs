namespace TypeSerialization._internal;

internal class SerializationFormat(string format, char open, char close, char sep)
{
    public static readonly SerializationFormat[] Values = [
        new("{0}({1})", '(', ')', '-'),
        new("{0}<{1}>", '<', '>', ','),
    ];

    public readonly string Format = format;
    public readonly char Open = open;
    public readonly char Close = close;
    public readonly char Sep = sep;
#if NETSTANDARD2_0
    public readonly string SepStr = sep.ToString();
#endif
}
