namespace MatchService.Models
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
    }
}

