# TypeSerialization [![NuGet version](https://badge.fury.io/nu/TypeSerialization.svg)](http://badge.fury.io/nu/TypeSerialization)
Serialization an object type to/from a string

## Features
* Generics support
* URI safe format

## Example 1: Serialization into a uri-safe format
```C#
var str = TypeSerializer.Serialize(typeof(Dictionary<int,string>), Formats.UriSafe);
Console.WriteLine(str);

// Console output: 
// Dictionary(Int32-String)
```

## Example 2: Serialization into a code-like format
```C#
var str = TypeSerializer.Serialize(typeof(Dictionary<int,string>), Formats.CodeLike);
Console.WriteLine(str);

// Console output: 
// Dictionary<Int32,MyCustomClass>
```

## Example 3: Deserialization
```C#
var deserializer = new TypeDeserializer(/* add your possible types for resolving */);
var type = deserializer.Deserialize("Dictionary(Int32-String)");
Console.WriteLine(type);

// Console output: 
// System.Collections.Generic.Dictionary`2[System.Int32,System.String]
```

## Example 4: JSON serialization
```C#
var jsonOptions = new JsonSerializerOptions();
jsonOptions.Converters.Add(new JsonTypeConverter(deserializer));

var obj = new MyCustomClass() { MyTypeProperty = typeof(List<int>) };
var json = JsonSerializer.Serialize(obj, jsonOptions);
Console.WriteLine(json);

// Console output: 
// {"MyTypeProperty":"List(Int32)"}
```

## Example 5: JSON deserialization
```C#
var objFromJson = JsonSerializer.Deserialize<MyCustomClass>(json, jsonOptions);
Console.WriteLine(objFromJson.MyTypeProperty);

// Console output: 
// System.Collections.Generic.List`1[System.Int32]
```


[Program.cs](https://github.com/mustaddon/TypeSerialization/tree/master/Examples/Program.cs)