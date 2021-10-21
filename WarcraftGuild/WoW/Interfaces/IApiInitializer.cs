using System.Collections.Generic;
using System.Threading.Tasks;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.WoW.Interfaces
{
    public interface IApiInitializer
    {
        Task InitAll();
        Task InitAchievements();
        Task InitRealms();
        Task InitCharacterDatas();
        Task InitApiDatas();

        Task InitGuild(string realmSlug, string guildSlug);
    }
}