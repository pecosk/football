namespace FootballLeague.Models
{
    public class Set
    {
        public int Id { get; set; }        

        public int Team1Score { get; set; }

        public int Team2Score { get; set; }

        public virtual Match Match { get; set; }
    }
}