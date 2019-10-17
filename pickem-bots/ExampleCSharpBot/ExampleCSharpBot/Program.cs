using System.Threading.Tasks;
using Raven.Client.Documents;

namespace ExampleCSharpBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var dataLoader = new EspnDataLoader();
            var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "pickem_predictions" 
            };
            documentStore.Initialize();

            using var session = documentStore.OpenAsyncSession();
            int count = 0;

            await foreach(var entry in dataLoader.LoadEspnDataAsync())
            {
                await session.StoreAsync(entry);
                count++;

                if(count > 0 && count % 1000 == 0)
                    await session.SaveChangesAsync();
            }

            //var picker = new Picker();
            //await picker.MakePicksAsync();
        }
    }
}