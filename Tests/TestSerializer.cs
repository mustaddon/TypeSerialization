namespace Tests
{
    public class TestSerializer
    {
        [Test]
        public void RuntimeType()
        {
            var result = TypeSerializer.Serialize(1.GetType().GetType());
            Assert.That(result, Is.EqualTo("Type"));
        }

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

        [Test]
        public void Many()
        {
            var result = TypeSerializer.Serialize(new[] { typeof(bool?), typeof(Dictionary<int, string>) });
            Assert.That(result, Is.EqualTo("Nullable(Boolean)-Dictionary(Int32-String)"));
        }

        [Test]
        public void ManyOpen()
        {
            var result = TypeSerializer.Serialize(new[] { typeof(List<>), typeof(Dictionary<,>) });
            Assert.That(result, Is.EqualTo("List()-Dictionary(-)"));
        }

        [Test]
        public void ManyNullable()
        {
            var result = TypeSerializer.Serialize(new[] { typeof(int), null, typeof(string) });
            Assert.That(result, Is.EqualTo("Int32--String"));
        }
    }
}