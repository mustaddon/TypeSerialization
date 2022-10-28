using TypeSerialization;


// Example 1: Serialization
var str = TypeSerializer.Serialize(typeof(Dictionary<int, string>));
Console.WriteLine(str);


// Example 2: Deserialization
var deserializer = new TypeDeserializer(new[] {
    // types pool for resolving
    typeof(int).Assembly, typeof(List<>).Assembly,
}.SelectMany(x => x.GetTypes()));

var type = deserializer.Deserialize("Dictionary(Int32-String)");
Console.WriteLine(type);