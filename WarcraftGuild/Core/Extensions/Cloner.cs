using System.IO;
using System.Xml.Serialization;

namespace WarcraftGuild.Core.Extensions
{
    public static class Cloner
    {
        public static T Clone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(ms);
            }
        }
    }
}
