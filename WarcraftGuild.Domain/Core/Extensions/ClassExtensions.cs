using System.Xml.Serialization;

namespace WarcraftGuild.Domain.Core.Extensions
{
    public static class ClassExtensions
    {
        public static T Clone<T>(this T obj)
        {
            MemoryStream ms = new();
            XmlSerializer serializer = new(obj.GetType());
            serializer.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            return (T)serializer.Deserialize(ms);
        }

        public static bool IsClone<T>(this T obj, T other)
        {
            MemoryStream ms1 = new();
            MemoryStream ms2 = new();
            XmlSerializer serializer = new(obj.GetType());
            serializer.Serialize(ms1, obj);
            serializer.Serialize(ms2, other);
            ms1.Seek(0, SeekOrigin.Begin);
            ms2.Seek(0, SeekOrigin.Begin);
            byte[] objBytes = ms1.ToArray();
            byte[] otherBytes = ms2.ToArray();
            if (objBytes.Length != otherBytes.Length)
                return false;
            for (int i = 0; i < objBytes.Length && i < otherBytes.Length; i++)
                if (objBytes[i] != otherBytes[i])
                    return false;
            return true;
        }
    }
}