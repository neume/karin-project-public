using YamlDotNet.Serialization;
namespace Karin.Helpers.Serialization;

public static class Deserializer
{
    public static T Deserialize<T>(string path)
    {
        var deserializer = new DeserializerBuilder().Build();
        var yaml = File.ReadAllText(path);
        T def = deserializer.Deserialize<T>(yaml);

        return def;
    }
}
