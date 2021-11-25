using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WarcraftGuild.WoWHeadApi.Xml
{
    public abstract class WoWHeadApiXmlResponse
    {
        public WoWHeadApiXmlResponse()
        {
            Xml = new XDocument();
        }

        public XDocument Xml { get; set; }

        public bool IsValid { get { return Xml.Root.Elements().Select(x => x.Element("error")).Any(); } }

        public string Error 
        { 
            get 
            {
                if (IsValid)
                    return string.Empty;
                StringBuilder builder = new();
                foreach (XElement element in Xml.Root.Elements().Select(x => x.Element("error")))
                    builder.AppendLine(element.Value);
                return builder.ToString();
            }
        }
    }
}