using System.Threading.Tasks;
using ExampleCSharpBot.PickemApi.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace ExampleCSharpBot.PickemApi
{
    public class PickEmClient
    {
        private const string PickemBaseUrl = "https://streamhead.duckdns.org/p-api/tst/api";
        private readonly string leagueCode;
        
        private string jwt;

        public PickEmClient(string leagueCode)
        {
            this.leagueCode = leagueCode;
        }

        public async Task<Player> GetPlayerAsync(string username)
        {
            var request = new RestRequest("{leagueCode}/players/{username}");
            request.AddUrlSegment("leagueCode", leagueCode);
            request.AddUrlSegment("username", username);

            return await GetFromApi<Player>(request);

        }

        public async Task<PlayerScoreboard> GetPlayerScoreboardAsync(int currentWeekRef, string playerTag)
        {
            var request = new RestRequest("/{leagueCode}/{currentWeekRef}/{playerTag}/scoreboard");
            request.AddUrlSegment("leagueCode", leagueCode);
            request.AddUrlSegment("currentWeekRef", currentWeekRef);
            request.AddUrlSegment("playerTag", playerTag);

            return await GetFromApi<PlayerScoreboard>(request);

        }

        private async Task<T> GetFromApi<T>(RestRequest request)
            where T : class, new()
        {
            var client = new RestClient(PickemBaseUrl)
            {
                Authenticator = new JwtAuthenticator(jwt)
            };

            return await client.GetAsync<T>(request);
        }

        public async Task<UserLoggedIn> Login(string username, string password)
        {
            var request = new RestRequest("/auth/login");
            var userCredentials = new UserCredentials { UserName = username, Password = password};

            var userLoggedIn =  await PostToApi<UserLoggedIn>(request, userCredentials);
            this.jwt = userLoggedIn.Token;

            return userLoggedIn;
        }

        private async Task<T> PostToApi<T>(RestRequest request, object payload)
            where T : class, new()
        {
            var client = new RestClient(PickemBaseUrl);
            request.AddJsonBody(payload);
            return await client.PostAsync<T>(request);
        }

        public async Task<PlayerPick> MakePick(int currentWeekRef, string playerTag, int gameId, PlayerPickUpdate pick)
        {
            var request = new RestRequest("/{leagueCode}/{currentWeekRef}/{playerTag}/scoreboard/{gameId}/pick");
            request.AddUrlSegment("leagueCode", leagueCode);
            request.AddUrlSegment("currentWeekRef", currentWeekRef);
            request.AddUrlSegment("playerTag", playerTag);
            request.AddUrlSegment("gameId", gameId);

            return await PutToApi<PlayerPick>(request, pick);
        }

        private async Task<T> PutToApi<T>(RestRequest request, object payload)
            where T : class, new()
        {
            var client = new RestClient(PickemBaseUrl)
            {
                Authenticator = new JwtAuthenticator(jwt)
            };

            request.AddJsonBody(payload);
            return await client.PutAsync<T>(request);
        }
    }
}