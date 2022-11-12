using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TypeSerialization.Json;

public class JsonTypeConverter : JsonConverterFactory
{
    public JsonTypeConverter(TypeDeserializer typeDeserializer)
    {
        _typeConverter = new InnerConverter(typeDeserializer ?? throw new ArgumentNullException(nameof(typeDeserializer)));
    }

    readonly InnerConverter _typeConverter;
    static readonly Type _type = typeof(Type);

    public override bool CanConvert(Type typeToConvert)
    {
        return _type.IsAssignableFrom(typeToConvert);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return _typeConverter;
    }

    class InnerConverter : JsonConverter<Type>
    {
        public InnerConverter(TypeDeserializer typeDeserializer)
        {
            _typeDeserializer = typeDeserializer;
        }

        readonly TypeDeserializer _typeDeserializer;

        public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return _typeDeserializer.Deserialize(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.Serialize());
        }
    }
}
