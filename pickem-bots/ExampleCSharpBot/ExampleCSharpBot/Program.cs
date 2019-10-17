using System.Threading.Tasks;

namespace ExampleCSharpBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var dataLoader = new EspnDataLoader();
            await dataLoader.LoadEspnDataAsync();

            //var picker = new Picker();
            //await picker.MakePicksAsync();
        }
    }
}