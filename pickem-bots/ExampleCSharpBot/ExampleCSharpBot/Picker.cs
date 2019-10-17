using System.Linq;
using System.Threading.Tasks;
using ExampleCSharpBot.PickemApi;
using ExampleCSharpBot.PickemApi.Models;

namespace ExampleCSharpBot
{
    public class Picker
    {
        const string PickemUserName = "???";
        const string PickemPassword = "???";
        const string PickemBotLeagueCode = "BOTS-NCAAF-19";

        public async Task MakePicksAsync()
        {
            var client = new PickEmClient(PickemBotLeagueCode);

            // authenticate
            var userLoggedIn = await client.Login(PickemUserName, PickemPassword);

            // get player tag for this user in the league
            var player = await client.GetPlayerAsync(PickemUserName);

            // get pickem games for current week
            var botLeague = userLoggedIn.Leagues.Single(l => l.LeagueCode == PickemBotLeagueCode);
            var playerScoreboard = await client.GetPlayerScoreboardAsync(botLeague.CurrentWeekRef, player.PlayerTag);

            var pick = PickTypes.Home;

            foreach (var gamePickScoreboard in playerScoreboard.GamePickScoreboards)
            {
                // THIS IS WHERE YOUR MAGIC PICK LOGIC GOES. 

                // pick your pick
                var pickUpdate = new PlayerPickUpdate { Pick = pick };

                var playerPickResult = await client.MakePick(botLeague.CurrentWeekRef,
                    player.PlayerTag, gamePickScoreboard.GameId,
                    pickUpdate);

                pick = pick == PickTypes.Home ? PickTypes.Away : PickTypes.Home;
            }
        }
    }
}
