namespace FootballLeague.Models.Tournament
{
    public class TournamentSet
    {
        public TournamentSet()
        {
            Team1Score = 0;
            Team2Score = 0;
            IsPlayed = false;
        }

        public int Id { get; set; }        

        public int Team1Score { get; set; }

        public int Team2Score { get; set; }

        public bool IsPlayed { get; set; }

        public TournamentMatch TournamentMatch { get; set; }
    }
}