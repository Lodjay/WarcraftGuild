using System.Collections.Generic;
using System.Threading.Tasks;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.WoW.Interfaces
{
    public interface IApiInitializer
    {
        Task Init();

        Task InitGuild(string realmSlug, string guildSlug);
    }
}