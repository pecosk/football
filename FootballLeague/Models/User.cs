using System.Collections.Generic;

namespace FootballLeague.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Match> Matches { get; set; }
    }
}