using System.Text.Json;
using TypeSerialization.Json;

namespace Tests;

public class TestJson
{
    public TestJson()
    {
        _jsonOptions = new JsonSerializerOptions();
        _jsonOptions.Converters.Add(new JsonTypeConverter(new TypeDeserializer()));
    }

    readonly JsonSerializerOptions _jsonOptions;

    [Test]
    public void Null()
    {
        Type? type = null;
        var json = JsonSerializer.Serialize(type, _jsonOptions);
        var result = JsonSerializer.Deserialize<Type>(json, _jsonOptions);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void RuntimeType()
    {
        var type = 1.GetType().GetType();
        var json = JsonSerializer.Serialize(type, _jsonOptions);
        var result = JsonSerializer.Deserialize<Type>(json, _jsonOptions);
        Assert.That(result, Is.EqualTo(typeof(Type)));
    }

    [Test]
    public void Single()
    {
        var type = typeof(string);
        var json = JsonSerializer.Serialize(type, _jsonOptions);
        var result = JsonSerializer.Deserialize<Type>(json, _jsonOptions);
        Assert.That(result, Is.EqualTo(type));
    }

    [Test]
    public void Open()
    {
        var type = typeof(Dictionary<,>);
        var json = JsonSerializer.Serialize(type, _jsonOptions);
        var result = JsonSerializer.Deserialize<Type>(json, _jsonOptions);
        Assert.That(result, Is.EqualTo(type));
    }

    [Test]
    public void Many()
    {
        var types = new[] { typeof(string), null, typeof(int[]), typeof(List<int?>) };
        var json = JsonSerializer.Serialize(types, _jsonOptions);
        var result = JsonSerializer.Deserialize<Type[]>(json, _jsonOptions);
        Assert.That(result, Is.EqualTo(types));
    }

}