using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TypeSerialization.Json;

public class JsonTypeConverter(
    TypeDeserializer typeDeserializer,
    Formats serializeFormat = Formats.UriSafe,
    Func<Type, string>? serializeNameProvider = null)
    : JsonConverter<Type>
{
    readonly Func<Type, string> _nameGetter = serializeNameProvider ?? (static t => t.Name);
    readonly TypeDeserializer _typeDeserializer = typeDeserializer ?? throw new ArgumentNullException(nameof(typeDeserializer));

    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return _typeDeserializer.Deserialize(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.Serialize(serializeFormat, _nameGetter));
    }
}
