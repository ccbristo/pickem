using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Dom;

namespace ExampleCSharpBot
{
    public class EspnDataLoader
    {
        private const string _baseUrl = "http://www.espn.com/college-football/statistics/teamratings/_/year/{0}";
        public async IAsyncEnumerable<FootballPowerIndexEntry> LoadEspnDataAsync()
        {
            const int startYear = 2015;

            var config = Configuration.Default.WithDefaultLoader();

            for (int year = startYear; year <= DateTime.Today.Year; year++)
            {
                var context = BrowsingContext.New(config);
                string yearUrl = string.Format(_baseUrl, year);
                var document = await context.OpenAsync(yearUrl);

                var weekSelector = (IHtmlSelectElement)document.QuerySelector("select.tablesm");

                foreach (var weekOption in weekSelector.Options)
                {
                    if (string.Equals(weekOption.Text, "Preseason", StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    int weekNumber = 0;
                    if (weekOption.Text == "Current" && year == DateTime.Today.Year)
                        continue;
                    else if(weekOption.Text == "Current")
                        weekNumber = GetWeekNumber((IHtmlOptionElement)weekOption.NextElementSibling) + 1;
                    else
                        weekNumber = GetWeekNumber(weekOption);

                    string weekUrl = weekOption.Value;

                    Console.WriteLine($"Week {weekNumber} - {year}");

                    document = await context.OpenAsync("http:" + weekUrl);

                    var rows = document.QuerySelectorAll("tr.oddrow, tr.evenrow")
                                    .Cast<IHtmlTableRowElement>();

                    int previousRank = 0;
                    foreach (var row in rows)
                    {
                        string rankText = row.Cells[0].TextContent;

                        int rank;
                        if (string.IsNullOrWhiteSpace(rankText))
                            rank = previousRank;
                        else
                            rank = int.Parse(row.Cells[0].TextContent);
                        previousRank = rank;

                        var team = row.Cells[1].FirstChild.TextContent;
                        var fpi = decimal.Parse(row.Cells[7].TextContent);

                        var entry = new FootballPowerIndexEntry(year, weekNumber, rank, team, fpi);
                        yield return entry;
                    }
                }
            }
        }

        static readonly Regex WeekNumberMatcher = new Regex(@"Week\s*(?<week>\d+)");
        private int GetWeekNumber(IHtmlOptionElement weekOption)
        {
            var match = WeekNumberMatcher.Match(weekOption.Text);
            System.Diagnostics.Debug.Assert(match.Success);

            var weekGroup = match.Groups["week"];
            System.Diagnostics.Debug.Assert(weekGroup.Success);
            return int.Parse(weekGroup.Value);
        }
    }
}