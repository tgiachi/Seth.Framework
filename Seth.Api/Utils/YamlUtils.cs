using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Seth.Api.Utils
{
    public static class YamlUtils
    {
        private static readonly IDeserializer Deserializer =
            new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

        private static readonly ISerializer Serializer =
            new SerializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();


        public static string ToYaml(this object obj)
        {
            return Serializer.Serialize(obj);
        }

        public static T FromYaml<T>(this string str)
        {
            try
            {
                return Deserializer.Deserialize<T>(str);
            }
            catch
            {
                return default;
            }
        }


        public static object FromYaml(this string str, Type type)
        {
            try
            {
                return Deserializer.Deserialize(str, type);
            }
            catch
            {
                return null;
            }
        }
    }
}