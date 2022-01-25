namespace WarcraftGuild.Domain.Interfaces
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