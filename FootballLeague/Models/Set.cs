using FootballLeague.Models.Tournament;
namespace FootballLeague.Models
{
    public class Set
    {
        public Set()
        {
            Team1Score = 0;
            Team2Score = 0;
        }

        public int Id { get; set; }        

        public int Team1Score { get; set; }

        public int Team2Score { get; set; }

        public Match Match { get; set; }        
    }
}