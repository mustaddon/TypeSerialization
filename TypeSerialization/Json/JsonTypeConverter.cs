using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TypeSerialization.Json;

public class JsonTypeConverter(
    TypeDeserializer typeDeserializer, 
    Formats serializeFormat = Formats.UriSafe, 
    Func<Type, string>? serializeNameProvider = null)
    : JsonConverterFactory
{
    readonly InnerConverter _typeConverter = new(
        typeDeserializer ?? throw new ArgumentNullException(nameof(typeDeserializer)),
        serializeFormat,
        serializeNameProvider ?? (static t => t.Name));
    
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(Type).IsAssignableFrom(typeToConvert);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return _typeConverter;
    }

    class InnerConverter(
        TypeDeserializer typeDeserializer, 
        Formats format, 
        Func<Type, string> nameGetter) 
        : JsonConverter<Type>
    {
        public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return typeDeserializer.Deserialize(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.Serialize(format, nameGetter));
        }
    }
}
