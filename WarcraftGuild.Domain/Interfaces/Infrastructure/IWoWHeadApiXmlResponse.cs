using System.Xml.Linq;

namespace WarcraftGuild.Domain.Interfaces
{
    public interface IWoWHeadApiXmlResponse
    {
        string Error { get; }
        bool IsValid { get; }
        XDocument Xml { get; set; }
    }
}