namespace WarcraftGuild.Domain.Interfaces.Infrastructure
{
    public interface IWoWHeadApiReader
    {
        TXml GetXmlAsync<TXml>(string query, string additionalParams = "") where TXml : IWoWHeadApiXmlResponse, new();
    }
}