using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace WarcraftGuild.WoWHeadApi.Xml
{
    public class ItemXml : WoWHeadApiXmlResponse
    {
        public ItemXml() : base()
        {
        }

        public ulong Id
        {
            get
            {
                XElement element = Xml.Root.Elements().Select(x => x.Element("item")).FirstOrDefault();
                if (element != null)
                {
                    XAttribute attribute = element.Attributes().FirstOrDefault(x => x.Name == "Id");
                    if (attribute != null)
                    {
                        if (ulong.TryParse(attribute.Value, out ulong id))
                            return id;
                    }
                }
                return 0;
            }
        }

        public string Name
        {
            get
            {
                XElement element = Xml.Root.Elements().Select(x => x.Element("name")).FirstOrDefault();
                if (element != null)
                {
                    return element.Value;
                }
                return string.Empty;
            }
        }
    }
}