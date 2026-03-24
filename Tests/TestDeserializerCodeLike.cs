using System.Reflection;

namespace Tests;

public class TestDeserializerCodeLike
{
    readonly TypeDeserializer _deserializer = new();


    [Test]
    public void String()
    {
        var result = _deserializer.Deserialize("String");
        Assert.That(result, Is.EqualTo(typeof(string)));
    }

    [Test]
    public void Nullable()
    {
        var result = _deserializer.Deserialize("bool?");
        Assert.That(result, Is.EqualTo(typeof(bool?)));
    }

    [Test]
    public void Arrays()
    {
        var result = _deserializer.Deserialize("string[]");
        Assert.That(result, Is.EqualTo(typeof(string[])));
    }

    [Test]
    public void Dictionary()
    {
        var result = _deserializer.Deserialize("Dictionary<int,string>");
        Assert.That(result, Is.EqualTo(typeof(Dictionary<int, string>)));
    }

    [Test]
    public void DictionaryOpen()
    {
        var result = _deserializer.Deserialize("Dictionary<,>");
        Assert.That(result, Is.EqualTo(typeof(Dictionary<,>)));
    }

    [Test]
    public void ListOpen()
    {
        var result = _deserializer.Deserialize("List()");
        Assert.That(result, Is.EqualTo(typeof(List<>)));
    }

    [Test]
    public void Many()
    {
        var result = _deserializer.DeserializeMany("bool?,Dictionary<Int32,String>");
        Assert.That(result, Is.EqualTo(new[] { typeof(bool?), typeof(Dictionary<int, string>) }));
    }

    [Test]
    public void ManyOpen()
    {
        var result = _deserializer.DeserializeMany("List<>,Dictionary<,>");
        Assert.That(result, Is.EqualTo(new[] { typeof(List<>), typeof(Dictionary<,>) }));
    }

    [Test]
    public void ManyNullable()
    {
        var result = _deserializer.DeserializeMany("Int32,,String");
        Assert.That(result, Is.EqualTo(new[] { typeof(int), null, typeof(string) }));
    }

    [Test]
    public void Custom()
    {
        var deserializer = new TypeDeserializer(
            Assembly.GetExecutingAssembly().GetTypes());

        var result = deserializer.Deserialize("List<IEnumerable<CustomClass>>");
        Assert.That(result, Is.EqualTo(typeof(List<IEnumerable<CustomClass>>)));
    }

    [Test]
    public void WithFormatErrors()
    {
        var result = _deserializer.Deserialize(" Dictionary< string, Dictionary<int, int? >> ");
        Assert.That(result, Is.EqualTo(typeof(Dictionary<string, Dictionary<int, int?>>)));
    }
}