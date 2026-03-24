using Examples;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using TypeSerialization;
using TypeSerialization.Json;

// Example 1: Serialization
var str = TypeSerializer.Serialize(typeof(Dictionary<int, MyCustomClass>), Formats.UriSafe);
Console.WriteLine(str);

// Example 2: Deserialization
var deserializer = new TypeDeserializer(
    /* add your possible types for resolving */
    Assembly.GetExecutingAssembly().GetTypes()
);

var type = deserializer.Deserialize("Dictionary<int,MyCustomClass>");
Console.WriteLine(type);


// Example 3: Json Serialization
var jsonOptions = new JsonSerializerOptions();
jsonOptions.Converters.Add(new JsonTypeConverter(deserializer));

var json = JsonSerializer.Serialize(type, jsonOptions);
Console.WriteLine(json);


// Example 4: Json Deserialization
var typeFromJson = JsonSerializer.Deserialize<Type>(json, jsonOptions);
Console.WriteLine(typeFromJson);