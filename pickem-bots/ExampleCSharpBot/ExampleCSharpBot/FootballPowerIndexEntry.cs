namespace ExampleCSharpBot
{
    public class FootballPowerIndexEntry
    {
        public string Id { get; set; }
        public int Year { get; }
        public int WeekNumber { get; }
        public int Rank { get; set; }
        public string Team { get; set; }
        public decimal Fpi { get; set; }

        public FootballPowerIndexEntry(int year, int weekNumber, int rank, string team, decimal fpi)
        {
            Year = year;
            WeekNumber = weekNumber;
            Rank = rank;
            Team = team;
            Fpi = fpi;
        }
    }
}