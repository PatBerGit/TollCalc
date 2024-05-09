using Newtonsoft.Json;
using System.Text.Json;

namespace TollCalculator.Model
{
    public class SholidayApiDay
    {
        [JsonProperty("datum")]
        public string? Date { get; set; }
        [JsonProperty("veckodag")]
        public string? WeekDay { get; set; }
        [JsonProperty("arbetsfri dag")]
        public string? WorkFreeDay { get; set; }
        [JsonProperty("röd dag")]
        public string? Holiday { get; set; }
        [JsonProperty("vecka")]
        public string? Week { get; set; }
        [JsonProperty("dag i vecka")]
        public string? DayInWeek { get; set; } 
        [JsonProperty("helgdag")]
        public string? HolidayName { get; set; }
        [JsonProperty("namnsdag")]
        public string[]? NameDay { get; set; }
        [JsonProperty("flaggdag")]
        public string? FlagDay { get; set; }
    }
}
