using System.Text.Json;
using TypeSerialization;
using TypeSerialization.Json;


// Example 1: Serialization
var str = TypeSerializer.Serialize(typeof(Dictionary<int, string>));
Console.WriteLine(str);


// Example 2: Deserialization
var deserializer = new TypeDeserializer(/* add your possible types for resolving */);
var type = deserializer.Deserialize("Dictionary(Int32-String)");
Console.WriteLine(type);


// Example 3: Json Serialization
var jsonOptions = new JsonSerializerOptions();
jsonOptions.Converters.Add(new JsonTypeConverter(deserializer));

var json = JsonSerializer.Serialize(type, jsonOptions);
Console.WriteLine(type);


// Example 4: Json Deserialization
var typeFromJson = JsonSerializer.Deserialize<Type>(json, jsonOptions);
Console.WriteLine(typeFromJson);