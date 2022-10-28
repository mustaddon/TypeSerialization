namespace Tests
{
    public class TestSerializer
    {
        [Test]
        public void String()
        {
            var result = TypeSerializer.Serialize(typeof(string));
            Assert.That(result, Is.EqualTo("String"));
        }

        [Test]
        public void Nullable()
        {
            var result = TypeSerializer.Serialize(typeof(bool?));
            Assert.That(result, Is.EqualTo("Nullable(Boolean)"));
        }

        [Test]
        public void Array()
        {
            var result = TypeSerializer.Serialize(typeof(string[]));
            Assert.That(result, Is.EqualTo("Array(String)"));
        }

        [Test]
        public void Dictionary()
        {
            var result = TypeSerializer.Serialize(typeof(Dictionary<int,string>));
            Assert.That(result, Is.EqualTo("Dictionary(Int32-String)"));
        }

        [Test]
        public void DictionaryOpen()
        {
            var result = TypeSerializer.Serialize(typeof(Dictionary<,>));
            Assert.That(result, Is.EqualTo("Dictionary(-)"));
        }

        [Test]
        public void ListOpen()
        {
            var result = TypeSerializer.Serialize(typeof(List<>));
            Assert.That(result, Is.EqualTo("List()"));
        }
    }
}