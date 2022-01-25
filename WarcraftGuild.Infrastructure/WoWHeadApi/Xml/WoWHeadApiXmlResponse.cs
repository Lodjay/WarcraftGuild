using System.Text;
using System.Xml.Linq;
using WarcraftGuild.Domain.Interfaces;

namespace WarcraftGuild.Infrastructure.WoWHeadApi.Xml
{
    public abstract class WoWHeadApiXmlResponse : IWoWHeadApiXmlResponse
    {
        public WoWHeadApiXmlResponse()
        {
            Xml = new XDocument();
        }

        public XDocument Xml { get; set; }

        public bool IsValid
        { get { return Xml.Root.Elements().Select(x => x.Element("error")).Any(); } }

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