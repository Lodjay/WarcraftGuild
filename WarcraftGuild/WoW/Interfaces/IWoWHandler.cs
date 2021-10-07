using System.Threading.Tasks;
using WarcraftGuild.WoW.Models;

namespace WarcraftGuild.WoW.Interfaces
{
    public interface IWoWHandler
    {
        Task<Guild> GetGuild(string realmName, string guildName);

        Task<Guild> GetGuild(string realmName, string guildName, bool forceRefresh);

        Task Init();
    }
}