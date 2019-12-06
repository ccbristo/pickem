using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;

namespace ExampleCSharpBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // TODO [ccb] Get scores of past games and compare matchups.

            //var dataLoader = new EspnDataLoader();
            //var documentStore = new DocumentStore
            //{
            //    Urls = new[] { "http://localhost:8080" },
            //    Database = "pickem_predictions" 
            //};
            //documentStore.Initialize();

            //using var session = documentStore.OpenAsyncSession();
            //int count = 0;

            //await foreach(var entry in dataLoader.LoadEspnDataAsync())
            //{
            //    await session.StoreAsync(entry);
            //    count++;

            //    if(count > 0 && count % 1000 == 0)
            //        await session.SaveChangesAsync();
            //}

            var config = new ConfigurationBuilder()
                .AddJsonFile("privatesettings.json", false, true)
                .Build();

            var picker = new Picker(config["PickemUserName"], config["PickemPassword"]);
            await picker.MakePicksAsync();
        }
    }
}