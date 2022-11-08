namespace Tests
{
    public class TestDeserializer
    {
        readonly TypeDeserializer _deserializer = new(new[] {
            typeof(int).Assembly,
            typeof(List<>).Assembly,
        }.SelectMany(x => x.GetTypes()));


        [Test]
        public void String()
        {
            var result = _deserializer.Deserialize("String");
            Assert.That(result, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void Nullable()
        {
            var result = _deserializer.Deserialize("Nullable(Boolean)");
            Assert.That(result, Is.EqualTo(typeof(bool?)));
        }

        [Test]
        public void Arrays()
        {
            var result = _deserializer.Deserialize("Array(String)");
            Assert.That(result, Is.EqualTo(typeof(string[])));
        }

        [Test]
        public void Dictionary()
        {
            var result = _deserializer.Deserialize("Dictionary(Int32-String)");
            Assert.That(result, Is.EqualTo(typeof(Dictionary<int, string>)));
        }

        [Test]
        public void DictionaryOpen()
        {
            var result = _deserializer.Deserialize("Dictionary(-)");
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
            var result = _deserializer.DeserializeMany("Nullable(Boolean)-Dictionary(Int32-String)");
            Assert.That(result, Is.EqualTo(new[] { typeof(bool?), typeof(Dictionary<int, string>) }));
        }

        [Test]
        public void ManyOpen()
        {
            var result = _deserializer.DeserializeMany("List()-Dictionary(-)");
            Assert.That(result, Is.EqualTo(new[] { typeof(List<>), typeof(Dictionary<,>) }));
        }

        [Test]
        public void RegistrationOpenless()
        {
            var type = typeof(Dictionary<string, Dictionary<int, int?>>);
            var deserializer = new TypeDeserializer(new[] { type });
            var result = deserializer.Deserialize("Dictionary(String-Dictionary(Int32-Nullable(Int32)))");
            Assert.That(result, Is.EqualTo(type));
        }

        [Test]
        public void RegistrationAdditional()
        {
            var deserializer = new TypeDeserializer(Array.Empty<Type>());
            var type = typeof(Dictionary<string, Dictionary<int, int?>>);
            deserializer.Register(type);
            var result = deserializer.Deserialize("Dictionary(String-Dictionary(Int32-Nullable(Int32)))");
            Assert.That(result, Is.EqualTo(type));
        }
    }
}