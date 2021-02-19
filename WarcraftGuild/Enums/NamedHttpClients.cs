using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarcraftGuild.Enums
{
    public enum NamedHttpClients
    {
        [EnumCode("ApiClient")]
        ApiClient,
        [EnumCode("AuthClient")]
        AuthClient
    }
}
