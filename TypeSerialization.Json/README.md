# TypeSerialization.Json [![NuGet version](https://badge.fury.io/nu/TypeSerialization.svg)](http://badge.fury.io/nu/TypeSerialization)
Serialization an object type to/from a JSON


## Example 1: Deserialization
```C#
var deserializer = new TypeDeserializer(/* add your possible types for resolving */);
var jsonOptions = new JsonSerializerOptions();
jsonOptions.Converters.Add(new JsonTypeConverter(deserializer));

var type = JsonSerializer.Deserialize<Type>("\"Dictionary(Int32-String)\"", jsonOptions);
Console.WriteLine(type);

// Console output: 
// System.Collections.Generic.Dictionary`2[System.Int32,System.String]
```

## Example 2: Serialization
```C#
var json = JsonSerializer.Serialize(typeof(Dictionary<int,string>), jsonOptions);
Console.WriteLine(json);

// Console output: 
// "Dictionary(Int32-String)"
```