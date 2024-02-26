namespace Tests
{
    public class TestFormats
    {
        [Test]
        public void AllFormats()
        {
            foreach (var format in Enum.GetValues(typeof(Formats)).Cast<Formats>())
            {
                var deserializer = new TypeDeserializer(new[] {
                    typeof(int).Assembly,
                    typeof(List<>).Assembly,
                }.SelectMany(x => x.GetTypes()), format);

                foreach (var type in new[] {
                    typeof(int),
                    typeof(bool?),
                    typeof(string[]),
                    typeof(Dictionary<,>),
                    typeof(Dictionary<int,string>),
                })
                {
                    var str = TypeSerializer.Serialize(type, format);
                    var val = deserializer.Deserialize(str);
                    Assert.That(val, Is.EqualTo(type));
                }
            }
        }
    }
}