namespace WarcraftGuild.Domain.Interfaces
{
    public interface IWebClient
    {
        Task<IApiResponse> MakeApiRequestAsync(string path);

        Task<IApiResponse> RequestAccessTokenAsync();

        Task<IApiResponse> RequestUserAuthorizeAsync();
    }
}