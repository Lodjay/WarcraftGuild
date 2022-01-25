using WarcraftGuild.Domain.Core.Extensions;

namespace WarcraftGuild.Domain.Core.Enums
{
    public enum NamedHttpClients
    {
        [EnumCode("ApiClient")]
        ApiClient,

        [EnumCode("AuthClient")]
        AuthClient
    }
}