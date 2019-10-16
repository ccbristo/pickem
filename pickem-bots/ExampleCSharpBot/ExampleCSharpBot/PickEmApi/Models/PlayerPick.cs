namespace ExampleCSharpBot.PickemApi.Models
{
    public class PlayerPick
    {
        public PickTypes Pick { get; set; }
        public int GamesPending { get; set; }
        public int GamesPicked { get; set; }
    }
}
