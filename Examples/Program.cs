using Examples;
using System.Reflection;
using System.Text.Json;
using TypeSerialization;
using TypeSerialization.Json;


// Example 1: Serialization into a uri-safe format
var str = TypeSerializer.Serialize(typeof(Dictionary<int, MyCustomClass>), Formats.UriSafe);
Console.WriteLine(str);


// Example 2: Deserialization
var deserializer = new TypeDeserializer()
    /* add your possible types for resolving */
    .Register(Assembly.GetExecutingAssembly().GetTypes());

var type = deserializer.Deserialize("Dictionary<int,MyCustomClass>");
Console.WriteLine(type);


// Example 3: JSON serialization
var jsonOptions = new JsonSerializerOptions();
jsonOptions.Converters.Add(new JsonTypeConverter(deserializer));

var obj = new MyCustomClass() { MyTypeProperty = typeof(List<int>) };
var json = JsonSerializer.Serialize(obj, jsonOptions);
Console.WriteLine(json);


// Example 4: JSON deserialization
var objFromJson = JsonSerializer.Deserialize<MyCustomClass>(json, jsonOptions);
Console.WriteLine(objFromJson.MyTypeProperty);