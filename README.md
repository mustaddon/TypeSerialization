# TypeSerialization [![NuGet version](https://badge.fury.io/nu/TypeSerialization.svg)](http://badge.fury.io/nu/TypeSerialization)
Serialization an object type to/from a string

## Features
* Generics support
* URI safe format

## Example 1: Serialization
```C#
var str = TypeSerializer.Serialize(typeof(Dictionary<int,string>));
Console.WriteLine(str);

// Console output: 
// Dictionary(Int32-String)
```

## Example 2: Deserialization
```C#
var deserializer = new TypeDeserializer(/* add your possible types for resolving */);
var type = deserializer.Deserialize("Dictionary(Int32-String)");
Console.WriteLine(type);

// Console output: 
// System.Collections.Generic.Dictionary`2[System.Int32,System.String]
```

[Program.cs](https://github.com/mustaddon/TypeSerialization/tree/master/Examples/Program.cs)