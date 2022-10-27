# TypeSerialization
Serialization an object type to/from a string

## Features
* Generics support
* URI safe format

## Example 1: Serialization
```C#
var str = TypeSerializer.Serialize(typeof(Dictionary<int,string>));
Console.WriteLine(str);
```

*Console output:*
```
Dictionary(Int32-String)
```

## Example 2: Deserialization
```C#
var deserializer = new TypeDeserializer(new[] {
    // types pool for resolving
    typeof(int).Assembly, typeof(List<>).Assembly,
}.SelectMany(x => x.GetTypes()));

var type = deserializer.Deserialize("Dictionary(Int32-String)");
```