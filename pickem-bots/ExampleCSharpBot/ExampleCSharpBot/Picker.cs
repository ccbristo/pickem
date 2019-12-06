using System;
using System.Linq;
using System.Threading.Tasks;
using ExampleCSharpBot.PickemApi;
using ExampleCSharpBot.PickemApi.Models;

namespace ExampleCSharpBot
{
    public class Picker
    {
        private readonly string _username;
        private readonly string _password;
        const string PickemBotLeagueCode = "BOTS-NCAAF-19";

        public Picker(string username, string password)
        {
            this._username = username;
            this._password = password;
        }

        public async Task MakePicksAsync()
        {
            var client = new PickEmClient(PickemBotLeagueCode);

            // authenticate
            var userLoggedIn = await client.Login(_username, _password);

            // get player tag for this user in the league
            var player = await client.GetPlayerAsync(_username);

            // get pickem games for current week
            var botLeague = userLoggedIn.Leagues.Single(l => l.LeagueCode == PickemBotLeagueCode);
            var playerScoreboard = await client.GetPlayerScoreboardAsync(botLeague.CurrentWeekRef, player.PlayerTag);

            foreach (var gamePickScoreboard in playerScoreboard.GamePickScoreboards)
            {

                PickTypes pick = GetPick(gamePickScoreboard);
                var pickUpdate = new PlayerPickUpdate { Pick = pick };

                var playerPickResult = await client.MakePick(botLeague.CurrentWeekRef,
                    player.PlayerTag, gamePickScoreboard.GameId,
                    pickUpdate);
            }
        }

        private PickTypes GetPick(GameScoreboard scoreboard)
        {
            return (scoreboard.HomeTeamLongName, scoreboard.AwayTeamLongName) switch
            {
                // always pick NC State, UNC, and CU regardless of opponent
                ("NC State", _) => PickTypes.Home,
                (_, "NC State") => PickTypes.Away,
                ("North Carolina", _) => PickTypes.Home,
                (_, "North Carolina") => PickTypes.Away,
                ("Colorado", _) => PickTypes.Home,
                (_, "Colorado") => PickTypes.Away,
                
                // never pick Duke
                ("Duke", _) => PickTypes.Away,
                (_, "Duke") => PickTypes.Home,

                // when in doubt, the team that comes alphabetically first, by first character gets the pick
                _ => scoreboard.HomeTeamLongName[0] < scoreboard.AwayTeamLongName[0] ? PickTypes.Home : PickTypes.Away
            };
        }
    }
}
