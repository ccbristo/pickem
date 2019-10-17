namespace ExampleCSharpBot
{
    public class FootballPowerIndexEntry
    {
        public virtual int Rank { get; set; }
        public virtual string Team { get; set; }
        public virtual decimal Fpi { get; set; }

        public FootballPowerIndexEntry(int rank, string team, decimal fpi)
        {
            Rank = rank;
            Team = team;
            Fpi = fpi;
        }
    }
}